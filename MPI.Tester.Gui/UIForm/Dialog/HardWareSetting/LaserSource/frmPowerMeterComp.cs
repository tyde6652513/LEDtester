using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.Gui;
using MPI.Tester.DeviceCommon;
using System.Reflection;
using MPI.Tester.GuiComponent;


namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.LaserSource
{
    public partial class frmPowerMeterComp : Form
    {
        PowerMeterSettingData _pmSetting;
        PowerMeterSpec _pmSpec;
        public frmPowerMeterComp()
        {
            InitializeComponent();

            _pmSetting = new PowerMeterSettingData();
            
            _pmSpec = new PowerMeterSpec();

            UnitAmpData desc = new UnitAmpData();

            CmpClampA.DescriptionPropertyUpload(1, 1E-6, 1, "0.000", "mA");

            
        }

        public void SetPowerMeterSpec(PowerMeterSpec pmSpec )
        {
            if (pmSpec != null)
            {
                _pmSpec = pmSpec.Clone() as PowerMeterSpec;
                SetDoubleLimit(dinWaveLength, _pmSpec.WavelengthRange);
                var pRange = dBm2WConverter.GetPowerInW(_pmSpec.PowerRange);
                SetDoubleLimit(dinTarPow, pRange);
                if (_pmSpec.SMUSpec != null)
                {
                    pnlSMU.Visible = true;
                    dinBias.MaxValue = 10;
                    dinBias.MinValue = -10;
                    //dinClamp.MaxValue = 1E-3;
                    //dinClamp.MinValue = 1E-9;
                    dinPDGain.MaxValue = 99999999;
                    dinPDGain.MinValue = 0;
                }
                else
                {
                    pnlSMU.Visible = false;
                }
                //pnlTarPow.Visible = haveAtt;

                if (DataCenter._uiSetting.UserID == EUserID.MPI_DEMO)
                {
                    chkRecord.Visible = true;
                }
            }

        }

        public void SetPowerMeterSetting(PowerMeterSettingData pmSet)
        {
            if (pmSet != null)
            { _pmSetting = pmSet; }
            dinWaveLength.Value = _pmSetting.WaveLength;

            dinPDGain.Value = _pmSetting.PDGain;

            dinSysGain.Value = _pmSetting.SysGain;

            //dinClamp.Value = _pmSetting.Clamp;

            CmpClampA.UploadDataToUI(_pmSetting.Clamp, _pmSetting.MsrtUnit);

            dinBias.Value = _pmSetting.ForceValue;

            dinTarPow.Value = _pmSetting.TarPower;

            dinCheckLim.Value = _pmSetting.Tolerence;

            chkRecord.Checked = _pmSetting.RecordPower;

            

        }

        public PowerMeterSettingData GetPowerMeterSetting()
        {
            _pmSetting.WaveLength = dinWaveLength.Value;//考量有可能兩者支援的波長不同，在特定狀況下兩者會獨立設定

            _pmSetting.PDGain = dinPDGain.Value;

            _pmSetting.SysGain = dinSysGain.Value ;

            UnitAmpData uad = CmpClampA.GetDataFromUI();

            _pmSetting.Clamp = uad.Value;

            _pmSetting.MsrtUnit = uad.UnitEA.ToString();
            //_pmSetting.Clamp = dinClamp.Value;

            CmpClampA.UploadDataToUI(_pmSetting.Clamp, _pmSetting.MsrtUnit);

            _pmSetting.ForceValue = dinBias.Value;

            _pmSetting.Tolerence = dinCheckLim.Value ;

            _pmSetting.TarPower = dinTarPow.Value;

            chkRecord.Checked &= chkRecord.Visible;

            _pmSetting.RecordPower = chkRecord.Checked;
            PowerMeterSettingData pms = _pmSetting.Clone() as PowerMeterSettingData;
            return pms;
        }

        #region >>peivate method<<
        private void SetDoubleLimit(DevComponents.Editors.DoubleInput din, MinMaxValuePair<double> mmp)
        {
            din.MaxValue = mmp.Max;
            din.MinValue = mmp.Min;
        }
        #endregion

        private void btnMsrt_Click(object sender, EventArgs e)
        {
            PowerMeterSettingData pm = GetPowerMeterSetting();
            double val = AppSystem.RunPowerMeter(pm);
            string str = val.ToString("E3") + " W";
            lblResultPow.Text = str;
        }
    }
}
