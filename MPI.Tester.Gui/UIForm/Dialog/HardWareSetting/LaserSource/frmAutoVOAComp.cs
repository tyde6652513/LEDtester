using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.LaserSource
{
    public partial class frmAutoVOAComp : Form
    {
        AutoTuneVOASettingData _tuneSetting;
        public frmAutoVOAComp()
        {
            _tuneSetting = new AutoTuneVOASettingData();
            InitializeComponent();
        }

        public frmAutoVOAComp(AutoTuneVOASettingData avt)
            : this()
        {
            _tuneSetting = avt.Clone() as AutoTuneVOASettingData;
            SetAutoTuneVOASetting(_tuneSetting);
        }

        
        #region

        public void SetAutoTuneVOASetting(AutoTuneVOASettingData ats)
        {
            dinTuneTolerance.Value = ats.TuneVOATolerence;
            dinTriggerLim.Value = ats.TuneVOATriggerLimit;
        }
        public AutoTuneVOASettingData GetAutoVOASetting()
        {
            _tuneSetting.TuneVOATolerence = dinTuneTolerance.Value;
            _tuneSetting.TuneVOATriggerLimit = dinTriggerLim.Value;
            return _tuneSetting.Clone() as AutoTuneVOASettingData;
        }
        public void AssignAutoTuneVOABtn(System.EventHandler handler)
        {
            this.btnAutoSetAtt.Click += handler;
        }
        #endregion

        #region
        private void btnAutoSetAtt_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        private void dinTuneTolerance_Validated(object sender, EventArgs e)
        {
            double TuneVOATolerence = dinTuneTolerance.Value;
            double TuneVOATriggerLimit = dinTriggerLim.Value;
            if (TuneVOATolerence > TuneVOATriggerLimit)
            {
                dinTuneTolerance.Value = TuneVOATriggerLimit;
            }
        }

        private void dinTriggerLim_Validated(object sender, EventArgs e)
        {
            double TuneVOATolerence = dinTuneTolerance.Value;
            double TuneVOATriggerLimit = dinTriggerLim.Value;
            if (TuneVOATolerence > TuneVOATriggerLimit)
            {
                dinTuneTolerance.Value = TuneVOATriggerLimit;
            }
        }

    }

    
}
