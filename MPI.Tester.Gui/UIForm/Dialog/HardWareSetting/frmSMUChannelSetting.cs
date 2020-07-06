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

namespace MPI.Tester.Gui.UIForm.Dialog.HardWareSetting
{
    public partial class frmSMUChannelSetting : Form
    {

        ChannelConfig _channelConfig;
        ETesterFunctionType _func;
        ESourceMeterModel _smuModel;
        //ETesterSequenceType _seqType;

        public frmSMUChannelSetting()
        {
            InitializeComponent();

            _channelConfig = new ChannelConfig();
            _func = ETesterFunctionType.Multi_Die;
            _smuModel = ESourceMeterModel.K2600;


            (this.dgvChannelConfigTable.Columns["colModel"] as DataGridViewComboBoxColumn).Items.Add(ESourceMeterModel.K2600.ToString());//.AddRange(Enum.GetNames(typeof(ESourceMeterModel)));
            (this.dgvChannelConfigTable.Columns["colModel"] as DataGridViewComboBoxColumn).Items.Add(ESourceMeterModel.Persona.ToString());
            (this.dgvChannelConfigTable.Columns["colModel"] as DataGridViewComboBoxColumn).Items.Add(ESourceMeterModel.NONE.ToString());

            (this.dgvChannelConfigTable.Columns["colDevSrcCh"] as DataGridViewComboBoxColumn).Items.Add("A");
            (this.dgvChannelConfigTable.Columns["colDevSrcCh"] as DataGridViewComboBoxColumn).Items.Add("B");
            //cmbMode.Items.Add(Enum.GetNames(typeof(ETesterFunctionType)));
            cmbSequential.Items.AddRange(Enum.GetNames(typeof(ETesterSequenceType)));
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.DR2000.ToString());
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.DSPHD.ToString());
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.IT7321.ToString());
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.LDT1A.ToString());
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.RM3542.ToString());
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.T2001L.ToString());
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.N5700.ToString());
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.LDT3A200.ToString());
            //this.cmbSrcMeterModel.Items.Remove(ESourceMeterModel.K2400.ToString());
            //colModel

            UpdateDataToChannelConfigDgv();
        }
        public frmSMUChannelSetting(ChannelConfig chCfg,ETesterFunctionType func,ESourceMeterModel model):this()
        {
            intCol.Value = chCfg.ColXCount;
            intRow.Value = chCfg.RowYCount;

            SetConfig(chCfg, func, model);
        }


        #region >>public UI<<
        public void SetConfig(ChannelConfig chCfg, ETesterFunctionType func, ESourceMeterModel model)
        {
            _channelConfig = chCfg.Clone() as ChannelConfig;
            _func = func;
            _smuModel = model;

            cmbSequential.SelectedItem = _channelConfig.TesterSequenceType.ToString();

            UpdateDataToChannelConfigDgv();
        }

        public ChannelConfig GetChConfig()
        {
            if (SaveDataFromUi2ChCfg())
            {
                return _channelConfig;
            }
            else { return null; }
            
        }

        public void RefteshDGV()
        {
            RefreshChCfg();
            UpdateDataToChannelConfigDgv();
        }
        #endregion

        #region >>private UI<<
        private void UpdateDataToChannelConfigDgv()
        {
            
            this.dgvChannelConfigTable.SuspendLayout();

            this.dgvChannelConfigTable.Rows.Clear();

            this.dgvChannelConfigTable.AllowUserToAddRows = true;

            this.dgvChannelConfigTable.RowCount = this._channelConfig.ChannelCount + 1;

            switch (_func)
            {
                case ETesterFunctionType.Single_Die:
                case ETesterFunctionType.Multi_Die:
                    {
                        this.dgvChannelConfigTable.Columns["colDutCh"].HeaderText = "CH";

                        if (this._channelConfig.TesterSequenceType == ETesterSequenceType.Series)
                        {
                            // Series Type
                            this.dgvChannelConfigTable.Columns["colModel"].Visible = true;
                            this.dgvChannelConfigTable.Columns["colDevSrcCh"].Visible = true;
                            this.dgvChannelConfigTable.Columns["colIP"].Visible = true;
                            this.dgvChannelConfigTable.Columns["colComPort"].Visible = false;
                        }
                        else
                        {
                            // Parallel Type
                            this.dgvChannelConfigTable.Columns["colModel"].Visible = true;
                            this.dgvChannelConfigTable.Columns["colDevSrcCh"].Visible = true;

                            if (_smuModel == ESourceMeterModel.K2600)
                            {
                                this.dgvChannelConfigTable.Columns["colModel"].ReadOnly = true;
                                this.dgvChannelConfigTable.Columns["colIP"].Visible = true;
                                this.dgvChannelConfigTable.Columns["colComPort"].Visible = false;
                            }
                            else
                            {
                                this.dgvChannelConfigTable.Columns["colModel"].ReadOnly = false;
                                this.dgvChannelConfigTable.Columns["colIP"].Visible = false;
                                this.dgvChannelConfigTable.Columns["colComPort"].Visible = true;
                            }
                        }
                        break;
                    }
                case ETesterFunctionType.Multi_Terminal:
                    {
                        // Parallel Type
                        this.dgvChannelConfigTable.Columns["colDutCh"].HeaderText = "SMU";
                        this.dgvChannelConfigTable.Columns["colModel"].Visible = false;
                        this.dgvChannelConfigTable.Columns["colDevSrcCh"].Visible = false;
                        this.dgvChannelConfigTable.Columns["colIP"].Visible = true;
                        this.dgvChannelConfigTable.Columns["colComPort"].Visible = false;
                        break;
                    }
            }


            for (int i = 0; i < this._channelConfig.AssignmentTable.Count; i++)
            {
                this.dgvChannelConfigTable["colDutCh", i].Value = (i + 1);                                            // DUT-CH Number
                this.dgvChannelConfigTable["colModel", i].Value = this._channelConfig.AssignmentTable[i].SourceModel;                       // Device Model
                //this.dgvChannelConfigTable["devNum", i].Value = this._channelConfig.AssignmentTable[i].DeviceNumber + 1;                   // Device Number
                this.dgvChannelConfigTable["colDevSrcCh", i].Value = this._channelConfig.AssignmentTable[i].SourceCH;  // Device Channel
                this.dgvChannelConfigTable["colIP", i].Value = this._channelConfig.AssignmentTable[i].DeviceIpAddress;                 // Device IpAddress
                this.dgvChannelConfigTable["colComPort", i].Value = this._channelConfig.AssignmentTable[i].DeviceComPort;
            }

            intCol.Value = this._channelConfig.ColXCount;
            intRow.Value = this._channelConfig.RowYCount;

            this.dgvChannelConfigTable.AllowUserToAddRows = false;

            this.dgvChannelConfigTable.ResumeLayout();
        }

        private void RefreshChCfg()
        {
            _channelConfig.ColXCount = intCol.Value;
            _channelConfig.RowYCount = intRow.Value;
            _channelConfig.TesterSequenceType = 
                (ETesterSequenceType)Enum.Parse(typeof(ETesterSequenceType), this.cmbSequential.SelectedItem.ToString(), true);

            _channelConfig.SlotCount = 1;

            _channelConfig.AssignmentTable.Clear();
            int length = _channelConfig.ColXCount * _channelConfig.RowYCount;

            if (DataCenter._machineInfo.SourceMeterHWVersion != null && DataCenter._machineInfo.SourceMeterHWVersion.Contains("Keithley") &&
           (DataCenter._machineInfo.SourceMeterHWVersion.Contains("2602") ||
            DataCenter._machineInfo.SourceMeterHWVersion.Contains("2612") ||
            DataCenter._machineInfo.SourceMeterHWVersion.Contains("2636")))
            {
                for (int i = 0; i < length; ++i)
                {
                    ChannelAssignmentData chData = new ChannelAssignmentData();
                    int rest = i % 2;
                    int num = (int)Math.Floor(((double)i)/2);
                    chData.DeviceComPort = "";
                    chData.DeviceIpAddress = "192.168.50." + (2 + num).ToString("0");
                    chData.DeviceSerialNum = "";
                    chData.SourceCH = "A";
                    if (rest == 1)
                    {
                        chData.SourceCH = "B";
                    }
                    chData.SourceModel = _smuModel.ToString();
                    _channelConfig.AssignmentTable.Add(chData);
                }
            }
            else
            {

                for (int i = 0; i < length; ++i)
                {
                    ChannelAssignmentData chData = new ChannelAssignmentData();
                    chData.DeviceComPort = "";
                    chData.DeviceIpAddress = "192.168.50." + (2 + i).ToString("0");
                    chData.DeviceSerialNum = "";
                    chData.SourceCH = "A";
                    chData.SourceModel = _smuModel.ToString();
                    _channelConfig.AssignmentTable.Add(chData);
                }
            }
        }

        private bool SaveDataFromUi2ChCfg()
        {
            RefreshChCfg();

            int length = _channelConfig.ColXCount * _channelConfig.RowYCount;

            ChannelConfig chConfig = new ChannelConfig( _channelConfig.RowYCount,_channelConfig.ColXCount);
            chConfig.TesterSequenceType =
                (ETesterSequenceType)Enum.Parse(typeof(ETesterSequenceType), this.cmbSequential.SelectedItem.ToString(), true);

            bool ifPass = true; ;
            for (int i = 0; i < length; ++i)
            {
                string ip = this.dgvChannelConfigTable["colIP", i].Value.ToString();
                string smuCh = this.dgvChannelConfigTable["colDevSrcCh", i].Value.ToString();
                if (chConfig.CheckSMUChannel(ip, smuCh))
                {
                    ChannelAssignmentData chData = new ChannelAssignmentData();// _channelConfig.AssignmentTable[i];
                    chData.DeviceComPort = this.dgvChannelConfigTable["colComPort", i].Value.ToString();
                    chData.DeviceIpAddress = this.dgvChannelConfigTable["colIP", i].Value.ToString();
                    chData.DeviceSerialNum = "";
                    chData.SourceCH = this.dgvChannelConfigTable["colDevSrcCh", i].Value.ToString();
                    chData.SourceModel = this.dgvChannelConfigTable["colModel", i].Value.ToString();
                    this.dgvChannelConfigTable.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
                    chConfig.AssignmentTable.Add(chData);
                }
                else
                {
                    this.dgvChannelConfigTable.Rows[i].DefaultCellStyle.BackColor = Color.DarkOrange;
                    ifPass = false;
                    break;
                }
            }

            if (ifPass)
            {
                _channelConfig = chConfig.Clone() as ChannelConfig;
            }

            return ifPass;
        }
        #endregion

        #region >>UI<<
       
        private void cmbSequential_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefteshDGV();
        }
        private void intCol_ValueChanged(object sender, EventArgs e)
        {
            RefteshDGV();
        }

        private void intRow_ValueChanged(object sender, EventArgs e)
        {
            RefteshDGV();
        }
        #endregion
    }
}
