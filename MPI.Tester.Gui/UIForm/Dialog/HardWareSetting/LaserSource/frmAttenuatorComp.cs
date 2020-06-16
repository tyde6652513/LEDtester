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


namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.LaserSource
{
    public partial class frmAttenuatorComp : Form
    {
        AttenuatorSettingData _attSetting;
        AttenuatorSpec _attSpec;
        public frmAttenuatorComp()
        {
            InitializeComponent();
            _attSetting = new AttenuatorSettingData();
            _attSetting.PowerUnit = ELaserPowerUnit.dBm;
            chkPowerCtrl.Checked = false;
            _attSpec = new AttenuatorSpec();
            
            SetAttSetting(_attSetting);
        }
        #region >>public property <<

        #endregion

        #region >>public method <<
        public void SetAttSpec(AttenuatorSpec attSpec)
        {
            if (attSpec!= null)
            {
                _attSpec = attSpec.Clone() as AttenuatorSpec;
                //pnlPowerCtrlMode.Visible = _attSpec.HavePowerControlMode;//先不開放，後續再接到RDFun用設定的方式處理
                pnlPowerCtrlMode.Visible = false;
                SetDoubleLimit(numWaveLength, _attSpec.WavelengthRange);
                SetDoubleLimit(numAtt, _attSpec.AttenuationRange);
                if (_attSpec.HavePowerControlMode)
                {                    
                    var pRange = dBm2WConverter.GetPowerInW(_attSpec.PowerRange);
                    SetDoubleLimit(numOutPower, pRange);
                }
                pnlPowerMonitor.Visible = _attSpec.HavePowerControlMode;
                pnlAttMsrt.Visible = _attSpec.HavePowerControlMode;

                btnMsrt.Text = attSpec.HavePowerControlMode ?  "MsrtPower" : "Set";

                if (DataCenter._uiSetting.UserID == EUserID.MPI_DEMO)
                {
                    pnlPowerCtrlMode.Visible = true;
                }

            }                
            
        }

        public void SetAttSetting(AttenuatorSettingData attset)
        {
            if (attset != null)
            { _attSetting = attset.Clone() as AttenuatorSettingData; }

            if (_attSetting.PowerUnit == ELaserPowerUnit.dBm)
            {
                _attSetting.PowerUnit = ELaserPowerUnit.W;
                _attSetting.Power.Unit = "W";
                _attSetting.PowdBm2W();
            }

            numWaveLength.Value = _attSetting.WaveLength;
            numOutPower.Value = _attSetting.Power.Set;
            numAtt.Value = _attSetting.Attenuate.Set;
            chkPowerCtrl.Checked  = _attSetting.APMode == EAPMode.Pwoer;
            if (DataCenter._uiSetting.UserID == EUserID.MPI_DEMO)
            {
                pnlPowerCtrlMode.Visible = true;
            }
            else { pnlPowerCtrlMode.Visible = false; }
            chkPowRec.Checked = _attSetting.IsRecordPower;
            

            SetUIforControlMode();
        }

        public AttenuatorSettingData GeAttSetting()
        {
            _attSetting.WaveLength = numWaveLength.Value;
            _attSetting.Power.Set =  numOutPower.Value ;
            _attSetting.Attenuate.Set = numAtt.Value;
            _attSetting.PowerContorll = chkPowerCtrl.Checked;
            _attSetting.APMode = _attSetting.PowerContorll ? EAPMode.Pwoer : EAPMode.Attenuator;
            _attSetting.APowerOn = true;//全部到外層判定
            chkPowRec.Checked &= pnlPowerCtrlMode.Visible;
            _attSetting.IsRecordPower = chkPowRec.Checked;

            AttenuatorSettingData atts = _attSetting.Clone() as AttenuatorSettingData;

            if (atts.PowerUnit == ELaserPowerUnit.W)
            {
                atts.PowerUnit = ELaserPowerUnit.dBm;
                atts.Power.Unit = "dBm";
                atts.PowW2dBm();
            }
            return atts;
        }
        #endregion

        #region >>private method<<
        private void SetDoubleLimit(DevComponents.Editors.DoubleInput din ,MinMaxValuePair<double> mmp)
        {
            din.MaxValue = mmp.Max;
            din.MinValue = mmp.Min;
        }

        private void SetUIforControlMode()
        {
            if (_attSpec.HavePowerControlMode)
            {
                if (chkPowerCtrl.Checked)
                {
                    pnlAttPower.Visible = true;
                    pnlAtt.Visible = false;
                    //pnlPowerMonitor.Visible = true;
                    //chkPowRec.Enabled = true;
                }
                else
                {
                    pnlAttPower.Visible = false;
                    pnlAtt.Visible = true;
                    //pnlPowerMonitor.Visible = false;
                    //chkPowRec.Enabled = false;
                }
            }
            else
            {
                chkPowerCtrl.Checked = false;
            }
        }
        #endregion

        private void chkPower_CheckedChanged(object sender, EventArgs e)
        {
            SetUIforControlMode();
        }

        private void btnMsrt_Click(object sender, EventArgs e)
        {
            AttenuatorSettingData att = GeAttSetting();

            att.APowerOn = true;
            double val = AppSystem.RunAttMoniter(att);
            string str = val.ToString("E3") + " dBm";
            lblResultPow.Text = str;

        }
    }
}
