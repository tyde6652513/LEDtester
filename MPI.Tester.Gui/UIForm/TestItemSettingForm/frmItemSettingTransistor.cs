using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;
using System.Drawing;

namespace MPI.Tester.Gui
{
    public partial class frmItemSettingTransistor : Form, IConditionUICtrl, IConditionElecCtrl
    {
        private object _lockObj;

        private TestItemData _item;

        private bool _isAutoSelectForceRange;
        private bool _isAutoSelectMsrtRange;
        private bool _isEnableNPLC;
        private bool _isEnableFilter;

        private bool _isEnablePdDetector;
        private bool _isEnableSpt;
        private bool _isEnableSwitchChannel;
        private bool _isEnableMsrtForceValue;

        private int _sweepPoints;

        public frmItemSettingTransistor()
        {
            InitializeComponent();

            this.pnlSequenceOrder.Visible = false;

            this._lockObj = new object();

            this._item = new TransistorTestItem();

            foreach(var e in Enum.GetValues(typeof(ESMU)))
            {
                this.cmbSmuDrain.Items.Add(e);
                this.cmbSmuSource.Items.Add(e);
                this.cmbSmuGate.Items.Add(e);
                this.cmbSmuBluk.Items.Add(e);
            }

            this.cmbSmuDrain.SelectedItem = ESMU.SMU1;
            this.cmbSmuSource.SelectedItem = ESMU.SMU2;
            this.cmbSmuGate.SelectedItem = ESMU.SMU3;
            this.cmbSmuBluk.SelectedItem = ESMU.SMU4;

            this.cmbSensingMode.Items.Clear();
            this.cmbSensingMode.Items.Add(ESensingMode.Fixed);
            this.cmbSensingMode.Items.Add(ESensingMode.Limit);

            this.cmbSensingMode.SelectedItem = ESensingMode.Fixed;

            this._isAutoSelectForceRange = false;
            this._isAutoSelectMsrtRange = false;
            this._isEnableNPLC = false;
            this._isEnableFilter = false;
            this._isEnableSwitchChannel = false;
            this._isEnableMsrtForceValue = false;

            this.txtDrain.Text = string.Empty;
            this.txtSource.Text = string.Empty;
            this.txtGate.Text = string.Empty;
            this.txtBluk.Text = string.Empty;

            this.cmbSensingMode.SelectedIndexChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);
            this.chkIsTestOptical.CheckedChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);
            this.chkIsEnableDetector.CheckedChanged += new System.EventHandler(this.UpdateOpticalComponentEventHandler);

            this.dinPoints.ValueChanged += new System.EventHandler(this.UpdateTerminalSettingEventHandler);

            this.cmbSmuDrain.SelectedIndexChanged += new System.EventHandler(this.UpdateTerminalSmuNameEventHandler);
            this.cmbSmuSource.SelectedIndexChanged += new System.EventHandler(this.UpdateTerminalSmuNameEventHandler);
            this.cmbSmuGate.SelectedIndexChanged += new System.EventHandler(this.UpdateTerminalSmuNameEventHandler);
            this.cmbSmuBluk.SelectedIndexChanged += new System.EventHandler(this.UpdateTerminalSmuNameEventHandler);

            this.btnSeqOrderUp.Click += new System.EventHandler(this.UpdateSequenceOrderEventHandler);
            this.btnSeqOrderDown.Click += new System.EventHandler(this.UpdateSequenceOrderEventHandler);

            this.btnSeqOrderUp.Tag = "Up";
            this.btnSeqOrderDown.Tag = "Down";

            this.cmbSmuDrain.Tag = ETerminalName.Drain;
            this.cmbSmuSource.Tag = ETerminalName.Source;
            this.cmbSmuGate.Tag = ETerminalName.Gate;
            this.cmbSmuBluk.Tag = ETerminalName.Bluk;

            this.chkIsTestOptical.Checked = false;
            this.chkIsEnableDetector.Checked = false;

            this.dinForceDelay.Value = 0.0d;

            this.dinDetectorBiasVoltage.Value = 0.0d;
        }

        public frmItemSettingTransistor(TestItemDescription description) : this()
        {
            this.UpdateItemBoudary(description);
        }

        #region >>> Public Property <<<

        public bool IsAutoSelectForceRange
        {
            get { return this._isAutoSelectForceRange; }
            set { lock (this._lockObj) { this._isAutoSelectForceRange = value; } }
        }

        public bool IsAutoSelectMsrtRange
        {
            get;
            set;

        }

        public bool IsVisibleFilterCount
        {
            get;
            set;
        }

        public bool IsVisibleNPLC
        {
            get;
            set;
        }

        public bool IsVisibleAdvancedSettings
        {
            get;
            set;
        }

        public bool IsEnablePDDetector
        {
            get { return this._isEnablePdDetector; }
            set { lock (this._lockObj) { this._isEnablePdDetector = value; } }
        }

        public bool IsEnableSpectrometer
        {
            get { return this._isEnableSpt; }
            set { lock (this._lockObj) { this._isEnableSpt = value; } }
        }

        public bool IsEnableSwitchChannel
        {
            get { return this._isEnableSwitchChannel; }
            set { lock (this._lockObj) { this._isEnableSwitchChannel = value; } }
        }
   
        public bool IsEnableMsrtForceValue
        {
            get { return this._isEnableMsrtForceValue; }
            set { lock (this._lockObj) { this._isEnableMsrtForceValue = value; } }
        }

        public uint MaxSwitchingChannelCount
        {
            get;
            set;
        }

        #endregion

        #region >>> Private Method <<<

        private void UpdateItemBoudary(TestItemDescription description)
        {
            if (description == null || description.Count == 0)
            {
                return;
            }

          
        }

        private void UpdateDataEventHandler()
        {


        }

        private void SetTerminalSetting(ETerminalName name, ESMU smu)
        {
            if (smu != ESMU.None)
            {
                int index = (int)name;
                
                uint points = (uint)this.dinPoints.Value;

                frmTerminalSetting frm = new frmTerminalSetting(name, smu, points);

                frm.UpdateSettingDataToComponent((this._item as TransistorTestItem).TRTerminalDescription[index]);

                DialogResult result = frm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    (this._item as TransistorTestItem).TRTerminalDescription[index] = frm.GetSettingDataFromComponent();
                }

                frm.Dispose();

                frm.Close();
            }

            this.UpdateTerminalInfoDisplay();
        }

        private void UpdateOpticalComponentEventHandler(object sender, EventArgs e)
        {
            // SPT
            if (this.chkIsTestOptical.Checked)
            {
                this.pnlSensingMode.Enabled = true;

                this.pnlFixedTime.Enabled = true;

                this.pnlLimitTime.Enabled = true;

                if (this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Limit.ToString() || this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Limit02.ToString())
                {
                    this.pnlFixedTime.Enabled = false;

                    this.pnlLimitTime.Enabled = true;
                }
                else if (this.cmbSensingMode.SelectedItem.ToString() == ESensingMode.Fixed.ToString())
                {
                    this.pnlFixedTime.Enabled = true;

                    this.pnlLimitTime.Enabled = false;
                }
            }
            else
            {
                this.pnlSensingMode.Enabled = false;

                this.pnlFixedTime.Enabled = false;

                this.pnlLimitTime.Enabled = false;
            }

            // PD Detector
            if (this.chkIsEnableDetector.Checked)
            {
                this.pnlDetectorBiasVoltage.Enabled = true;

                this.pnlIsFixedMsrtRange.Enabled = true;

                this.pnlDetectorNPLC.Enabled = true;
            }
            else
            {
                this.pnlDetectorBiasVoltage.Enabled = false;

                this.pnlIsFixedMsrtRange.Enabled = false;

                this.pnlDetectorNPLC.Enabled = false;
            }
        }

        private void UpdateTerminalSettingEventHandler(object sender, EventArgs e)
        {
            int points = (int)this.dinPoints.Value;

			int index = 0;

            foreach (var data in (this._item as TransistorTestItem).TRTerminalDescription)
            {
				if (data.SMU != ESMU.None)
				{
					if (data.MsrtType == EMsrtType.FIMVSWEEP || data.MsrtType == EMsrtType.FVMISWEEP)
					{
                        if (data.SweepMode == ESweepMode.Custom || data.SweepMode == ESweepMode.Log)
						{
							int diffPoints = points - data.SweepCustomList.Count;

							if (diffPoints > 0)
							{
								data.SweepCustomList.AddRange(new double[diffPoints]);

                                data.SweepLogList.AddRange(new double[diffPoints]);
							}
							else if (diffPoints < 0)
							{
								int removeCount = Math.Abs(diffPoints);

								data.SweepCustomList.RemoveRange(data.SweepCustomList.Count - removeCount, removeCount);

                                data.SweepLogList.RemoveRange(data.SweepLogList.Count - removeCount, removeCount);
							}
						}
						else
						{
							if (points > 1)
							{
								double step = (data.SweepStop - data.SweepStart) / (points - 1);

								data.SweepStep = Math.Round(step, 3, MidpointRounding.AwayFromZero);
							}
							else
							{
								data.SweepStep = 0.0d;
							}
						}
					}
				}
				else
				{
					ETerminalName oldTerminalName = data.TerminalName;

					(this._item as TransistorTestItem).TRTerminalDescription[index] = new ElecTerminalSetting(oldTerminalName, ESMU.None);
				}

				index++;
            }

            this.UpdateTerminalInfoDisplay();
        }

        private void UpdateTerminalSmuNameEventHandler(object sender, EventArgs e)
        {
            ETerminalName senderTag = (ETerminalName)(sender as DevComponents.DotNetBar.Controls.ComboBoxEx).Tag;

			(this._item as TransistorTestItem).TRTerminalDescription[(int)senderTag].SMU = (ESMU)(sender as DevComponents.DotNetBar.Controls.ComboBoxEx).SelectedItem; 

			this.UpdateTerminalSettingEventHandler(null, null);
        }

        private void UpdateSequenceOrderEventHandler(object sender, EventArgs e)
        {
            if (this.ListBoxSeqOrder.SelectedIndex >= 0)
            {
                string tag = (sender as Button).Tag.ToString();

                ETerminalName selectedName = (ETerminalName)this.ListBoxSeqOrder.SelectedItem;

                int selectedIndex = (int)selectedName;

                if (selectedName != ETerminalName.None)
                {
                    int currentOrder = (this._item as TransistorTestItem).TRTerminalDescription[selectedIndex].SequenceOrder;

                    switch (tag)
                    {
                        case "Up":
                            {
                                if ((currentOrder - 1) >= 0)
                                {
                                    ETerminalName reoderName = (this._item as TransistorTestItem).GetTerminalNameByOrder(currentOrder - 1);
                                    (this._item as TransistorTestItem).TRTerminalDescription[selectedIndex].SequenceOrder--;
                                    (this._item as TransistorTestItem).TRTerminalDescription[(int)reoderName].SequenceOrder++;
                                }
                                
                                break;
                            }
                        case "Down":
                            {
                                if ((currentOrder + 1) < this.ListBoxSeqOrder.Items.Count)
                                {
                                    ETerminalName reoderName = (this._item as TransistorTestItem).GetTerminalNameByOrder(currentOrder + 1);
                                    (this._item as TransistorTestItem).TRTerminalDescription[selectedIndex].SequenceOrder++;
                                    (this._item as TransistorTestItem).TRTerminalDescription[(int)reoderName].SequenceOrder--;
                                }

                                break;
                            }

                    }

                    this.UpdateSequenceOrderDisplay();
                }
            }
        }

        private void UpdateTerminalInfoDisplay()
        {
            foreach (var data in (this._item as TransistorTestItem).TRTerminalDescription)
            {
                string info = "None";
                string strFunc = string.Empty;
                string strSrcSetting = string.Empty;
                string strMsrtSetting = string.Empty;

                if (data.SMU != ESMU.None)
                {
                    strFunc = data.Description + "\n";

                    if (data.MsrtType == EMsrtType.FIMVSWEEP || data.MsrtType == EMsrtType.FVMISWEEP)
                    {
                        if (data.SweepMode == ESweepMode.Linear)
                        {
                            strSrcSetting = string.Format("Mode = {0}\nStart = {1} {2}\nStep = {3} {4}\nStop = {5} {6}\n", data.SweepMode.ToString(),
                                                                                                                           data.SweepStart.ToString(), data.ForceUnit,
                                                                                                                           data.SweepStep.ToString(), data.ForceUnit,
                                                                                                                           data.SweepStop.ToString(), data.ForceUnit);
                        }
                        else
                        {
                            strSrcSetting = string.Format("Mode = {0}\n", data.SweepMode.ToString()); 
                        }
                    }
                    else
                    {
                        strSrcSetting = string.Format("Force Value = {0} {1}\n", data.ForceValue, data.ForceUnit);
                    }

                    strMsrtSetting = string.Format("Msrt Clamp = {0} {1}\n", data.MsrtProtection, data.MsrtUnit);

                    info = strFunc + strSrcSetting + strMsrtSetting;
                }

                switch (data.TerminalName)
                {
                    case ETerminalName.Drain:
                        {
                            this.txtDrain.Text = info;
                            break;
                        }
                    case ETerminalName.Source:
                        {
                            this.txtSource.Text = info;
                            break;
                        }
                    case ETerminalName.Gate:
                        {
                            this.txtGate.Text = info;
                            break;
                        }
                    case ETerminalName.Bluk:
                        {
                            this.txtBluk.Text = info;
                            break;
                        }
                }
            }
        }

        private void UpdateSequenceOrderDisplay()
        {
            this.ListBoxSeqOrder.SuspendLayout();
            
            this.ListBoxSeqOrder.Items.Clear();
            
            for (int order = 0; order < (this._item as TransistorTestItem).TRTerminalDescription.Length; order++)
            {
                ETerminalName name = (this._item as TransistorTestItem).GetTerminalNameByOrder(order);

                if (name != ETerminalName.None)
                {
                    this.ListBoxSeqOrder.Items.Add(name);
                }
            }

            this.ListBoxSeqOrder.ResumeLayout();
        }

        private void UpdatedgvDetectorMsrtRangeRowCount()
        {
            int point = (int)this.dinPoints.Value;

            if (this.dgvDetectorMsrtRange.RowCount < point)
            {
                this.dgvDetectorMsrtRange.Rows.Add(point - this.dgvDetectorMsrtRange.RowCount);
            }
            else if (this.dgvDetectorMsrtRange.RowCount > point)
            {
                for (int i = 0; i < this.dgvDetectorMsrtRange.RowCount - point; i++)
                {
                    this.dgvDetectorMsrtRange.Rows.RemoveAt(this.dgvDetectorMsrtRange.RowCount - 1);
                }
            }
            else
            {
                return;
            }

            for (int i = 0; i < point; i++)
            {
                this.dgvDetectorMsrtRange.Rows[i].HeaderCell.Value = (i + 1).ToString("000");

                this.dgvDetectorMsrtRange.DefaultCellStyle.Font = new Font("Microsoft JhengHei", 12);

                this.dgvDetectorMsrtRange.Rows[i].HeaderCell.Style.BackColor = Color.Green;
            }
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void frmItemSettingTransistor_Load(object sender, EventArgs e)
        {
            this.UpdatedgvDetectorMsrtRangeRowCount();
        }

        private void btnDrain_Click(object sender, EventArgs e)
        {
            this.SetTerminalSetting((ETerminalName)this.cmbSmuDrain.Tag, (ESMU)this.cmbSmuDrain.SelectedItem);
        }

        private void btnBluk_Click(object sender, EventArgs e)
        {
            this.SetTerminalSetting((ETerminalName)this.cmbSmuBluk.Tag, (ESMU)this.cmbSmuBluk.SelectedItem);
        }

        private void btnGate_Click(object sender, EventArgs e)
        {
            this.SetTerminalSetting((ETerminalName)this.cmbSmuGate.Tag, (ESMU)this.cmbSmuGate.SelectedItem);
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            this.SetTerminalSetting((ETerminalName)this.cmbSmuSource.Tag, (ESMU)this.cmbSmuSource.SelectedItem);
        }

        private void btnSeqOrderUp_Click(object sender, EventArgs e)
        {
            // 動做定閱在 UpdateSequenceOrderEventHandler(object sender, EventArgs e)
        }

        private void btnSeqOrderDown_Click(object sender, EventArgs e)
        {
            // 動做定閱在 UpdateSequenceOrderEventHandler(object sender, EventArgs e)
        }

        private void dinPoints_ValueChanged(object sender, EventArgs e)
        {
            this.UpdatedgvDetectorMsrtRangeRowCount();
        }

        private void chkIsEnableDetector_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIsEnableDetector.Checked)
            {
                this.chkIsFixedMsrtRange.TextColor = System.Drawing.Color.Green;
            }
            else
            {
                this.chkIsFixedMsrtRange.TextColor = System.Drawing.Color.Gray;
            }
        }

        private void chkIsFixedMsrtRange_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkIsFixedMsrtRange.Checked)
            {
                this.lblDetectorMsrtRange.Enabled = true;

                this.dinDetectorMsrtRange.Enabled = true;

                this.lblDetectorMsrtRangeUnit.Enabled = true;

                this.dgvDetectorMsrtRange.Enabled = false;
            }
            else
            {
                this.lblDetectorMsrtRange.Enabled = false;

                this.dinDetectorMsrtRange.Enabled = false;

                this.lblDetectorMsrtRangeUnit.Enabled = false;

                this.dgvDetectorMsrtRange.Enabled = true;
            }
        }

        #endregion

        #region >>> Public Method <<<

        public void RefreshUI()
        {
            if (!this._isEnableSpt)
            {
                this.grpOpticalSetting.Enabled = false;
                this.chkIsTestOptical.TextColor = this.chkIsEnableDetector.TextColor = System.Drawing.Color.Gray;
            }
            
            if (!this._isEnablePdDetector)
            {
                this.grpPDDetector.Enabled = false;
                this.chkIsEnableDetector.TextColor = this.chkIsFixedMsrtRange.TextColor = System.Drawing.Color.Gray;
            }
            
            this.UpdateTerminalInfoDisplay();

            this.UpdateSequenceOrderDisplay();
        }

        public bool CheckUI(out string msg)
        {
            msg = string.Empty;

            // Check Termial SMU
            foreach (var data in (this._item as TransistorTestItem).TRTerminalDescription)
            {
                if (!(this._item as TransistorTestItem).CheckTerminalSMU(data.TerminalName, data.SMU))
                {
                    msg = "SMU Selective Repeat";

                    return false;
                }
            }

            // Check Termial Setting
            foreach (var data in (this._item as TransistorTestItem).TRTerminalDescription)
            {
                if (data.SMU == ESMU.None)
                {
                    continue;
                }
                
                if (data.Description == string.Empty)
                {
                    msg = string.Format("Please set the parameter for \"{0}\".", data.TerminalName.ToString());

                    return false;
                }
            }


            return true;
        }

        public void UpdateCondtionDataToComponent(TestItemData data)
        {
            this._item = (data as TransistorTestItem).Clone() as TransistorTestItem;

            TransistorTestItem trItem = (this._item as TransistorTestItem);

            // Terminal setting
            this.cmbSmuDrain.SelectedItem = trItem.TRTerminalDescription[(int)ETerminalName.Drain].SMU;
            this.cmbSmuSource.SelectedItem = trItem.TRTerminalDescription[(int)ETerminalName.Source].SMU;
            this.cmbSmuGate.SelectedItem = trItem.TRTerminalDescription[(int)ETerminalName.Gate].SMU;
            this.cmbSmuBluk.SelectedItem = trItem.TRTerminalDescription[(int)ETerminalName.Bluk].SMU;

            // ElecSetting Data
            this.dinForceDelay.Value = trItem.TRProcessDelayTime;

            this.dinForceTime.Value = trItem.TRForceTime;

            this.dinTurnOffTime.Value = trItem.TRTurnOffTime;

            this.dinPoints.Value = trItem.TRSweepPoints;

            this.dinNPLC.Value = trItem.TRMsrtNPLC;

            // OpticalSetting Data
            this.chkIsTestOptical.Checked = trItem.TRIsTestOptical;

            this.cmbSensingMode.SelectedItem = trItem.TRSensingMode.ToString();

            this.dinFixIntegralTime.Value = trItem.TRFixIntegralTime;

            this.dinLimitIntegralTime.Value = trItem.TRLimitIntegralTime;

            // PD Detector
            this.chkIsEnableDetector.Checked = trItem.TRIsEnableDetector;

            this.dinDetectorBiasVoltage.Value = trItem.TRDetectorBiasVolt;

            this.chkIsFixedMsrtRange.Checked = trItem.TRIsFixedDetectorMsrtRange;

            this.dinDetectorMsrtRange.Value = trItem.TRDetectorMsrtRange;

            this.dinDetectorNPLC.Value = trItem.TRDetectorNPLC;

            this.dgvDetectorMsrtRange.Rows.Clear();

            int point = (int)trItem.TRSweepPoints;

            this.dgvDetectorMsrtRange.Rows.Add(point);

            this.dgvDetectorMsrtRange.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft JhengHei", 10, FontStyle.Bold);

            this.dgvDetectorMsrtRange.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;

            this.dgvDetectorMsrtRange.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            for (int i = 0; i < point; i++)
            {
                this.dgvDetectorMsrtRange.Rows[i].HeaderCell.Value = (i + 1).ToString("000");

                this.dgvDetectorMsrtRange.DefaultCellStyle.Font = new Font("Microsoft JhengHei", 10);

                this.dgvDetectorMsrtRange.Rows[i].HeaderCell.Style.BackColor = Color.Green;

                if (trItem.TRDetectorMsrtRangeList.Count > i)
                {
                    this.dgvDetectorMsrtRange[0, i].Value = trItem.TRDetectorMsrtRangeList[i];
                }
            }

            this.dgvDetectorMsrtRange.Enabled = trItem.TRIsFixedDetectorMsrtRange;

            if (this.chkIsEnableDetector.Checked)
            {
                this.chkIsFixedMsrtRange.TextColor = System.Drawing.Color.Green;
            }
            else
            {
                this.chkIsFixedMsrtRange.TextColor = System.Drawing.Color.Gray;
            }

            if (this.chkIsFixedMsrtRange.Checked)
            {
                this.lblDetectorMsrtRange.Enabled = true;

                this.dinDetectorMsrtRange.Enabled = true;

                this.lblDetectorMsrtRangeUnit.Enabled = true;

                this.dgvDetectorMsrtRange.Enabled = false;
            }
            else
            {
                this.lblDetectorMsrtRange.Enabled = false;

                this.dinDetectorMsrtRange.Enabled = false;

                this.lblDetectorMsrtRangeUnit.Enabled = false;

                this.dgvDetectorMsrtRange.Enabled = true;
            }

            this.UpdateTerminalInfoDisplay();

            this.UpdateSequenceOrderDisplay();
        }

        public TestItemData GetConditionDataFromComponent()
        {
            // Terminal setting
            (this._item as TransistorTestItem).TRTerminalDescription[(int)ETerminalName.Drain].SMU = (ESMU)this.cmbSmuDrain.SelectedItem;
            (this._item as TransistorTestItem).TRTerminalDescription[(int)ETerminalName.Source].SMU = (ESMU)this.cmbSmuSource.SelectedItem;
            (this._item as TransistorTestItem).TRTerminalDescription[(int)ETerminalName.Gate].SMU = (ESMU)this.cmbSmuGate.SelectedItem;
            (this._item as TransistorTestItem).TRTerminalDescription[(int)ETerminalName.Bluk].SMU = (ESMU)this.cmbSmuBluk.SelectedItem;
            
            // ElecSetting Data
            (this._item as TransistorTestItem).TRProcessDelayTime = this.dinForceDelay.Value;

            (this._item as TransistorTestItem).TRForceTime = this.dinForceTime.Value;

            (this._item as TransistorTestItem).TRTurnOffTime = this.dinTurnOffTime.Value;

            (this._item as TransistorTestItem).TRSweepPoints = (uint)this.dinPoints.Value;

            (this._item as TransistorTestItem).TRMsrtNPLC = this.dinNPLC.Value;

            // OpticalSetting Data
            (this._item as TransistorTestItem).TRIsTestOptical = this.chkIsTestOptical.Checked;

            (this._item as TransistorTestItem).TRSensingMode = (ESensingMode)this.cmbSensingMode.SelectedItem;

            (this._item as TransistorTestItem).TRFixIntegralTime = this.dinFixIntegralTime.Value;

            (this._item as TransistorTestItem).TRLimitIntegralTime = this.dinLimitIntegralTime.Value;

            // PD Detector
            (this._item as TransistorTestItem).TRIsEnableDetector = this.chkIsEnableDetector.Checked;

            (this._item as TransistorTestItem).TRIsFixedDetectorMsrtRange = this.chkIsFixedMsrtRange.Checked;

            (this._item as TransistorTestItem).TRDetectorBiasVolt = this.dinDetectorBiasVoltage.Value;

            (this._item as TransistorTestItem).TRDetectorMsrtRange = this.dinDetectorMsrtRange.Value;

            (this._item as TransistorTestItem).TRDetectorNPLC = this.dinDetectorNPLC.Value;

            (this._item as TransistorTestItem).TRDetectorMsrtRangeList.Clear();

            for (int i = 0; i < this.dgvDetectorMsrtRange.RowCount; i++)
            {
                if (this.chkIsFixedMsrtRange.Checked)
                {
                    //(this._item as TransistorTestItem).TRDetectorMsrtRangeList.Add(this.dinDetectorMsrtRange.Value);
                }
                else
                {
                    if (this.dgvDetectorMsrtRange[0, i].Value == null)
                    {
                        (this._item as TransistorTestItem).TRDetectorMsrtRangeList.Add(1e-3);
                    }
                    else
                    {
                        (this._item as TransistorTestItem).TRDetectorMsrtRangeList.Add(double.Parse(this.dgvDetectorMsrtRange[0, i].Value.ToString()));
                    }
                }
            }

            // Apply Setting Data
            (this._item as TransistorTestItem).TRApplyParameter();

            return this._item;
        }

        #endregion
    }
}
