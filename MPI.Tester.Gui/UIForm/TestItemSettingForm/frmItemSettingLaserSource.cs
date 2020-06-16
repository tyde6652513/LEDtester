using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using MPI.Tester.Gui.UIForm.Dialog.HardWareSetting.LaserSource;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingLaserSource : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private LaserSourceTestItem _item;

        bool _isPresetReady = false;

        bool _isNewCreateItem = true;
        Dictionary<string, int> _nameChDic = new Dictionary<string, int>();

        frmAttenuatorComp _frmAtt;
        frmOpticalSwitchComp _frmOS;
        frmPowerMeterComp _frmPM;
        frmAutoVOAComp _frmAVoa;
        int _d4Index = 0;
        int _sysCh = 0;

        bool IsD4Channel = false;
    
        public frmItemSettingLaserSource()
        {
            InitializeComponent();

            _item = new LaserSourceTestItem();

            _isNewCreateItem = true;

            int cnt = 0;
            _nameChDic = new Dictionary<string, int>();
            if (DataCenter._machineConfig.LaserSrcSysConfig.ChConfigList != null)
            {
                foreach (var ch in DataCenter._machineConfig.LaserSrcSysConfig.ChConfigList)
                {
                    if (!_nameChDic.ContainsKey(ch.ChannelName))
                    {
                        _nameChDic.Add(ch.ChannelName, ch.SysChannel);
                        if (ch.IsDefauleChannel)
                        { 
                            _d4Index = cnt;
                            IsD4Channel = true;
                        }
                        cnt++;
                    }
                }
            }

            cmbLaserChannel.Items.AddRange(_nameChDic.Keys.ToArray());

            #region>>_frmAtt<<

            _frmAtt = new frmAttenuatorComp();
            PrerSetSubUI(_frmAtt, grpAtt);
            _frmAtt.SetAttSetting(_item.LaserSourceSet.AttenuatorData);
            #endregion

            #region >>_frmOS<<
            _frmOS = new frmOpticalSwitchComp();
            PrerSetSubUI(_frmOS, grpOS);
            _frmOS.SetString("");            
            #endregion

            #region >>_frmPM<<
            _frmPM = new frmPowerMeterComp();
            PrerSetSubUI(_frmPM, grpPowerMeter);
            _frmPM.SetPowerMeterSetting(_item.LaserSourceSet.PowerMeterSetting);
            #endregion
            _frmAVoa = new frmAutoVOAComp(_item.LaserSourceSet.AutoTuneVOASetting);
            PrerSetSubUI(_frmAVoa, gpAutoTuneVOA);
            //_frmAVoa.btn.Click += new System.EventHandler(this.btnAutoSetAtt_Click);
            _frmAVoa.AssignAutoTuneVOABtn(new System.EventHandler(this.btnAutoSetAtt_Click));
            
            #region


            #endregion

            _isPresetReady = true;

            if (_d4Index >= 0 && _d4Index < cmbLaserChannel.Items.Count)
            {
                cmbLaserChannel.SelectedIndex = _d4Index;
            }

            #region
            gpAutoTuneVOA.Visible = _frmAtt.Enabled && _frmPM.Enabled;
            
            #endregion
        }


        public frmItemSettingLaserSource(TestItemDescription description) :this()
        {
            this.UpdateItemBoudary(description);
        }
        #region >>> Public Property <<<

        public bool IsAutoSelectForceRange{set;get;}

        public bool IsAutoSelectMsrtRange { set; get; }

        public bool IsVisibleFilterCount { set; get; }

        public bool IsVisibleNPLC{ set; get; }

        public bool IsEnableSwitchChannel{ set; get; }

        public bool IsEnableMsrtForceValue{ set; get; }

        public uint MaxSwitchingChannelCount { set; get; }
        #endregion

        #region >>private method <<
        private void PrerSetSubUI(Form frm, Control parent)
        {
            frm.TopLevel = false;
            frm.Parent = parent;
            frm.Dock = DockStyle.Fill;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Show();
            frm.Visible = true;
        }

        private void SetAttenuatorUI()
        {
            try
            {
                string chName = (string)cmbLaserChannel.SelectedItem;
                int sysCh = _nameChDic[chName];
                if (DataCenter._machineInfo.ChLaserSysSpecDic.ContainsKey(sysCh) && DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].AttSpec != null)
                {
                    _frmAtt.Enabled = true;

                    AttenuatorSpec attSpec = DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].AttSpec.Clone() as AttenuatorSpec;
                    if (DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].AttSpec.HavePowerControlMode)
                    {
                        MinMaxValuePair<double> range = AppSystem.GetAttPowRangeIndBm(sysCh);
                        attSpec.PowerRange = range;
                    }
                    _frmAtt.SetAttSpec(attSpec);
                    _item.LaserSourceSet.AttenuatorData.SysChannel = sysCh;
                    _frmAtt.SetAttSetting(_item.LaserSourceSet.AttenuatorData);
                }
                else
                {_frmAtt.Enabled = false;}
            }
            catch (Exception ex)
            {
                Console.WriteLine("[frmItemSettingLaserSource],SetAttenuatorUI(),Exception," + ex.Message);
            }
        }

        private void SetPowerMeterUI()
        {
            try
            {
                string chName = (string)cmbLaserChannel.SelectedItem;
                int sysCh = _nameChDic[chName];
                if (DataCenter._machineInfo.ChLaserSysSpecDic.ContainsKey(sysCh) && DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].PowerMeterSpec != null )
                {
                    _frmPM.Enabled = true;
                    bool haveAtt = DataCenter._machineInfo.ChLaserSysSpecDic.ContainsKey(sysCh) && DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].AttSpec != null;
                    _frmPM.SetPowerMeterSpec(DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].PowerMeterSpec);
                    _item.LaserSourceSet.PowerMeterSetting.SysChannel = sysCh;
                    _frmPM.SetPowerMeterSetting(_item.LaserSourceSet.PowerMeterSetting);
                }
                else { _frmPM.Enabled = false; }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("[frmItemSettingLaserSource],SetPowerMeterUI(),Exception," + ex.Message);
            }
        }

        private void SetAutoTuneVOA()
        {
            try
            {
                string chName = (string)cmbLaserChannel.SelectedItem;
                int sysCh = _nameChDic[chName];
                if (DataCenter._machineInfo.ChLaserSysSpecDic.ContainsKey(sysCh) && DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].PowerMeterSpec != null 
                    && DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].AttSpec != null)
                {
                    this._frmAVoa.Enabled = true;
                    bool haveAtt = DataCenter._machineInfo.ChLaserSysSpecDic.ContainsKey(sysCh) && DataCenter._machineInfo.ChLaserSysSpecDic[sysCh].AttSpec != null;
                    
                    _item.LaserSourceSet.PowerMeterSetting.SysChannel = sysCh;
                    _frmAVoa.SetAutoTuneVOASetting(_item.LaserSourceSet.AutoTuneVOASetting);
                    //_frmPM.SetPowerMeterSetting(_item.LaserSourceSet.PowerMeterSetting);
                }
                else { _frmPM.Enabled = false; }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[frmItemSettingLaserSource],SetPowerMeterUI(),Exception," + ex.Message);
            }
        }

        private string GetOSSet(int sysCh)
        {
            string str = "";
            try
            {
                if (DataCenter._machineConfig.LaserSrcSysConfig.ChConfigList != null)
                {
                    foreach (var chCfg in DataCenter._machineConfig.LaserSrcSysConfig.ChConfigList)
                    {
                        if (chCfg.OpticalSwitchList != null && chCfg.SysChannel == sysCh && chCfg.Enable)
                        {
                            foreach (var osCfg in chCfg.OpticalSwitchList)
                            {
                                if (osCfg.Enable)
                                {
                                    str += "Model:" + osCfg.OpticalSwitchModel.ToString() + "\nIP:" + osCfg.Address + "\nslot:" + osCfg.Slot.ToString() +
                                        "\nIn:" + osCfg.OpticalInputChannel.ToString() + ",Out:" + osCfg.OpticalOutputChannel.ToString() + "\n----------";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[frmItemSettingLaserSource],GetOSSet(),Exception," + ex.Message);
                str = "";
            }
            return str;
        }

        private void UpdateItemBoudary(TestItemDescription description)
        {
            if (description == null || description.Count == 0)
            {
                return;
            }

            foreach (var data in description.Property)
            {
                EItemDescription keyName = (EItemDescription)Enum.Parse(typeof(EItemDescription), data.PropertyKeyName);

                switch (keyName)
                {
                    case EItemDescription.WaitTime:
                        {
                            this.dinForceDelay.MaxValue = data.MaxValue;
                            this.dinForceDelay.MinValue = data.MinValue;
                            this.dinForceDelay.Value = data.DefaultValue;
                            this.dinForceDelay.DisplayFormat = data.Format;
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void AutoTuneVOA()
        {
            LaserSourceTestItem liten = GetConditionDataFromComponent() as LaserSourceTestItem;
            LaserSourceTestItem rliten = AppSystem.RunAutoLaserCompensate(liten);
            if (rliten != null)
            {
                UpdateCondtionDataToComponent(rliten);
            }
            else
            {
                Host.SetErrorCode(EErrorCode.LASER_AutoSetAttenuator_Fail_Err);
            }
        }
        #endregion

        #region >>public method<<
        public void RefreshUI()
        { 
        }
        public bool CheckUI(out string msg)
        {
            msg = "";
            return true;
        }
        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            _item = (data as LaserSourceTestItem).Clone() as LaserSourceTestItem;
            
            string key = _item.LaserSourceSet.ChName;
            if (_nameChDic.ContainsKey(key))
            { 
                cmbLaserChannel.SelectedItem = key;
            }

            this.dinForceDelay.Value = data.ElecSetting[0].ForceDelayTime;
            cmbLaserChannel.Enabled = false;
            _frmAtt.SetAttSetting(_item.LaserSourceSet.AttenuatorData);
            _frmPM.SetPowerMeterSetting(_item.LaserSourceSet.PowerMeterSetting);
            this._frmAVoa.SetAutoTuneVOASetting(_item.LaserSourceSet.AutoTuneVOASetting);
            _isNewCreateItem = false;
        }

        public TestItemData GetConditionDataFromComponent()
        {
            _item.ElecSetting[0].ForceDelayTime = this.dinForceDelay.Value;
            if (_frmAtt.Enabled &&  _frmAtt.GeAttSetting() != null)
            {
                _item.LaserSourceSet.AttenuatorData = _frmAtt.GeAttSetting();
                if (_item.LaserSourceSet.AttenuatorData != null)
                {
                    _item.LaserSourceSet.AttenuatorData.SysChannel = _sysCh;

                    if ( DataCenter._machineConfig.LaserSrcSysConfig.ChConfigList != null &&
                        DataCenter._machineConfig.LaserSrcSysConfig.ChConfigList.FindIndex(x => x.SysChannel == _sysCh) >=0)
                    {
                        int chID = DataCenter._machineConfig.LaserSrcSysConfig.ChConfigList.FindIndex(x => x.SysChannel == _sysCh);
                        //_item.LaserSourceSet.AttenuatorData.APowerOn = !DataCenter._machineConfig.LaserSrcSysConfig.ChConfigList[chID].IsDefauleChannel;
                    }
                }
            }
            else { _item.LaserSourceSet.AttenuatorData = null; }

            if (_frmPM.Enabled && _frmPM.GetPowerMeterSetting() != null)
            {
                _item.LaserSourceSet.PowerMeterSetting = _frmPM.GetPowerMeterSetting();
                if (_item.LaserSourceSet.PowerMeterSetting != null)
                {
                    _item.LaserSourceSet.PowerMeterSetting.SysChannel = _sysCh;
                }
            }
            else { _item.LaserSourceSet.PowerMeterSetting = null; }

            if (_frmAVoa.Enabled && _frmAVoa.GetAutoVOASetting() != null)
            {
                _item.LaserSourceSet.AutoTuneVOASetting = _frmAVoa.GetAutoVOASetting();
            }
            else
            { _item.LaserSourceSet.AutoTuneVOASetting = null; }

            if (_item.MsrtResult != null && _item.MsrtResult.Length >= 3)
            {
                _item.MsrtResult[2].Unit = "dBm";
            }

            return _item;
        }

        #endregion

        private void cmbLaserChannel_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_isPresetReady)
            {
                string chName = (string)cmbLaserChannel.SelectedItem;
                _sysCh = _nameChDic[chName];
                _item.LaserSourceSet.SysChannel = _sysCh;
                _item.LaserSourceSet.ChName = chName;
                SetAttenuatorUI();
                SetPowerMeterUI();
                string str = GetOSSet(_sysCh);
                _frmOS.SetString(str);
                _frmOS.SetSysChannel(_sysCh);
                btnAutoSetAtt.Visible = false;// _frmAtt.Enabled && _frmPM.Enabled;
                gpAutoTuneVOA.Visible = _frmAtt.Enabled && _frmPM.Enabled;
            }
        }        

        private void btnAutoSetAtt_Click(object sender, EventArgs e)
        {
            AutoTuneVOA();
        }

        private void btnAutoSetAtt_Click_1(object sender, EventArgs e)
        {

        }

       

       
    }
}
