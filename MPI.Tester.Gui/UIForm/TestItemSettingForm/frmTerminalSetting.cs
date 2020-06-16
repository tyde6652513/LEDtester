using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
    public partial class frmTerminalSetting : Form
    {
        private ElecTerminalSetting _setting;
        private ETerminalName _eName;
        private ESMU _eSMU;
        private uint _points;

        private string _forceUnit;
        private string _msrtUnit;

        public frmTerminalSetting()
        {
            InitializeComponent();

            this.btnConfirm.DialogResult = DialogResult.OK;

            this.btnCancel.DialogResult = DialogResult.Cancel;

            this.AcceptButton = this.btnConfirm;

            this.CancelButton = this.btnCancel;

            this.cmbTestSelected.Items.Clear();
			this.cmbTestSelected.Items.Add(new FunctionType(EMsrtType.FIMV, "Current Bias"));
			this.cmbTestSelected.Items.Add(new FunctionType(EMsrtType.FVMI, "Voltage Bias"));
            this.cmbTestSelected.Items.Add(new FunctionType(EMsrtType.FIMVSWEEP, "Current Sweep"));
            this.cmbTestSelected.Items.Add(new FunctionType(EMsrtType.FVMISWEEP, "Voltage Sweep"));

            this.cmbTestSelected.DisplayMember = "Name";

            this.cmbTestSelected.SelectedIndex = 0;

            this.cmbSweepMode.Items.Add(ESweepMode.Linear);
            this.cmbSweepMode.Items.Add(ESweepMode.Log);
            this.cmbSweepMode.Items.Add(ESweepMode.Custom);

            this.tabpBias.Visible = true;
            this.tabpSweep.Visible = false;
            this._eName = ETerminalName.Drain;
            this._eSMU = ESMU.None;

            this.cmbSweepMode.SelectedIndexChanged += new System.EventHandler(this.UpdateSweepEventHandler);
            this.dinStartValue.ValueChanged += new System.EventHandler(this.UpdateSweepEventHandler);
            this.dinEndValue.ValueChanged += new System.EventHandler(this.UpdateSweepEventHandler);

            this.cmbSweepMode.SelectedItem = ESweepMode.Linear;

            this._setting = new ElecTerminalSetting();
        }

        public frmTerminalSetting(ETerminalName name, ESMU smu, uint points) : this()
        {
            this._eSMU = smu;

            this._eName = name;

            this._points = points;

            this.UpdateUIComponent();
        }

        #region >>> Private Method <<<

        private void UpdateUIComponent()
        {
            switch ((EMsrtType)(this.cmbTestSelected.SelectedItem as FunctionType).MsrtType)
            {
                case EMsrtType.FIMV:
                    {
                        this.tabpBias.Visible = true;
                        this.tabpSweep.Visible = false;

                        this._forceUnit = "mA";
                        this._msrtUnit = "V";

                        this.dinForceValue.DisplayFormat = "0.000";
						this.dinForceValue.MinValue = -1500;
                        this.dinForceValue.MaxValue = 1500.0d;
                        this.lblForceValueUnit.Text = this._forceUnit;

                        this.dinMsrtClamp.DisplayFormat = "0.0";
						this.dinMsrtClamp.MinValue = 0.1;
                        this.dinMsrtClamp.MaxValue = 200.0d;
                        this.lblMsrtClampUnit.Text = this._msrtUnit;

                        break;
                    }
                case EMsrtType.FVMI:
                    {
                        this.tabpBias.Visible = true;
                        this.tabpSweep.Visible = false;

                        this._forceUnit = "V";
                        this._msrtUnit = "mA";

                        this.dinForceValue.DisplayFormat = "0.0";
                        this.dinForceValue.MinValue = -200;
                        this.dinForceValue.MaxValue = 200.0d;
                        this.lblForceValueUnit.Text = this._forceUnit;

                        this.dinMsrtClamp.DisplayFormat = "0.000";
                        this.dinMsrtClamp.MinValue = 0.001;
                        this.lblMsrtClampUnit.Text = this._msrtUnit;
                        this.dinMsrtClamp.MaxValue = 1500.0d;

                        break;
                    }
                case EMsrtType.FIMVSWEEP:
                    {
                        this.tabpBias.Visible = false;
                        this.tabpSweep.Visible = true;

                        this._forceUnit = "mA";
                        this._msrtUnit = "V";

                        this.dinStartValue.DisplayFormat = "0.000";
                        this.dinStartValue.MaxValue = 1500.0d;
                        this.dinStartValue.MinValue = -1500.0d;
                        this.lblStartValueUnit.Text = this._forceUnit;

                        this.dinEndValue.DisplayFormat = "0.000";
                        this.dinEndValue.MaxValue = 1500.0d;
                        this.dinEndValue.MinValue = -1500.0d;
                        this.lblEndValueUnit.Text = this._forceUnit;

                        this.lblStepValueUnit.Text = this._forceUnit;

                        this.dinMsrtClamp.DisplayFormat = "0.0";
                        this.dinMsrtClamp.MinValue = 0.1;
                        this.lblMsrtClampUnit.Text = this._msrtUnit;
                        this.dinMsrtClamp.MaxValue = 200.0d;

                        break;
                    }
                case EMsrtType.FVMISWEEP:
                    {
                        this.tabpBias.Visible = false;
                        this.tabpSweep.Visible = true;

                        this._forceUnit = "V";
                        this._msrtUnit = "mA";

                        this.dinStartValue.DisplayFormat = "0.0";
                        this.lblStartValueUnit.Text = this._forceUnit;
                        this.lblStepValueUnit.Text = this._forceUnit;
                        this.dinStartValue.MaxValue = 200.0d;
                        this.dinStartValue.MinValue = -200.0d;

                        this.dinEndValue.DisplayFormat = "0.0";
                        this.lblEndValueUnit.Text = this._forceUnit;
                        this.dinEndValue.MaxValue = 200.0d;
                        this.dinEndValue.MinValue = -200.0d;

                        this.dinMsrtClamp.DisplayFormat = "0.000";
                        this.dinMsrtClamp.MinValue = 0.001;
                        this.lblMsrtClampUnit.Text = this._msrtUnit;
                        this.dinMsrtClamp.MaxValue = 1500.0d;

                        break;
                    }
            }

            this.grpTitle.Text = string.Format("{0} ({1})", this._eName, this._eSMU.ToString());
        }

        private void UpdateSweepEventHandler(object sender, EventArgs e)
        {
            double stepValue = 0.0d;

            switch ((ESweepMode)this.cmbSweepMode.SelectedItem)
            {
                case ESweepMode.Linear:
                    {
                        this.dinStartValue.Enabled = true;
                        this.dinEndValue.Enabled = true;
                        this.txtDisplayStepValue.Enabled = true;

                        this.lblCustomList.Visible = false;
                        this.dgvCustomList.Visible = false;

                        if (this._points > 1)
                        {
                            stepValue = (this.dinEndValue.Value - this.dinStartValue.Value) / (this._points - 1);
                        }
                        else
                        {
                            stepValue = 0.0d;
                        }

                        this.txtDisplayStepValue.Text = stepValue.ToString("0.000");

                        break;
                    }
                case ESweepMode.Log:
                    {
                        this.dinStartValue.Enabled = true;
                        this.dinEndValue.Enabled = true;
                        this.txtDisplayStepValue.Enabled = true;

                        this.lblCustomList.Visible = false;
                        this.dgvCustomList.Visible = false;
                        
                        if (this.dinStartValue.Value * this.dinEndValue.Value < 0)
                        {
                            this.dinEndValue.Value = this.dinEndValue.Value * -1;
                        }

                        this.txtDisplayStepValue.Text = "Log Step";

                        break;
                    }
                case ESweepMode.Custom:
                    {
                        this.dinStartValue.Enabled = false;
                        this.dinEndValue.Enabled = false;
                        this.txtDisplayStepValue.Enabled = false;

                        this.lblCustomList.Visible = true;
                        this.dgvCustomList.Visible = true;

                        this.UpdateCustomSweepListToDgv();
                        
                        break;
                    }
            }
        }

        private void UpdateCustomSweepListToDgv()
        {
            this.dgvCustomList.SuspendLayout();

            this.dgvCustomList.Rows.Clear();

            for (int i = 0; i < this._points; i++)
            {
                this.dgvCustomList.Rows.Add();

                this.dgvCustomList.Rows[i].Cells[0].Value = (i + 1).ToString();
            }

            this.dgvCustomList.ResumeLayout();
        }

        #endregion

        #region >>> Public Method <<<

        public void UpdateSettingDataToComponent(ElecTerminalSetting data)
        {
            this._setting = data.Clone() as ElecTerminalSetting;
            
            int count = 0;

            foreach (var obj in this.cmbTestSelected.Items)
            {
                if ((obj as FunctionType).MsrtType == this._setting.MsrtType)
                {
                    this.cmbTestSelected.SelectedIndex = count;

                    break;
                }

                count++;
            }

            this.dinForceValue.Value = this._setting.ForceValue;

            this.dinMsrtClamp.Value = this._setting.MsrtProtection;

            this.cmbSweepMode.SelectedItem = this._setting.SweepMode;

            this.dinStartValue.Value = this._setting.SweepStart;

            this.dinEndValue.Value = this._setting.SweepStop;

            if (this._setting.SweepMode == ESweepMode.Custom)
            {
                for (int i = 0; i < this._points; i++)
                {
                    if (i < this._setting.SweepCustomList.Count)
                    {
                        this.dgvCustomList[1, i].Value = this._setting.SweepCustomList[i];
                    }
                    else
                    {
                        this.dgvCustomList[1, i].Value = 0.0d;
                    }
                }
            }
            else if (this._setting.SweepMode == ESweepMode.Log)
            {
                this._setting.SweepLogList = MPI.Tester.Maths.CoordTransf.LogScale(this.dinStartValue.Value, this.dinEndValue.Value, this._points);
            }
        }

        public ElecTerminalSetting GetSettingDataFromComponent()
        {
            this._setting.SMU = this._eSMU;

            this._setting.TerminalName = this._eName;

            this._setting.MsrtType = (this.cmbTestSelected.SelectedItem as FunctionType).MsrtType;

            this._setting.Description = (this.cmbTestSelected.SelectedItem as FunctionType).Name;

            this._setting.ForceValue = this.dinForceValue.Value;

            this._setting.MsrtRange = this.dinMsrtClamp.Value;

            this._setting.MsrtProtection = this.dinMsrtClamp.Value;

            this._setting.SweepMode = (ESweepMode)this.cmbSweepMode.SelectedItem;

            this._setting.SweepStart = this.dinStartValue.Value;

            double stepValue = 0.0d;

            double.TryParse(this.txtDisplayStepValue.Text, out stepValue);

            this._setting.SweepStep = stepValue;

            this._setting.SweepStop = this.dinEndValue.Value;

            this._setting.SweepRiseCount = this._points;

            if (this._setting.SweepMode == ESweepMode.Custom)
            {
                this._setting.SweepCustomList.Clear();

                for (int i = 0; i < this.dgvCustomList.RowCount; i++)
                {
					double tempValue = 0.0d;

					if (this.dgvCustomList[1, i].Value != null)
					{
						double.TryParse(this.dgvCustomList[1, i].Value.ToString(), out tempValue);
					}

                    this._setting.SweepCustomList.Add(tempValue);
                }
            }
            else if (this._setting.SweepMode == ESweepMode.Log)
            {
                this._setting.SweepLogList = MPI.Tester.Maths.CoordTransf.LogScale(this.dinStartValue.Value, this.dinEndValue.Value, this._points);
            }

            this._setting.ForceUnit = this._forceUnit;
            this._setting.MsrtUnit = this._msrtUnit;

            return this._setting;
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Click Event 訂閱在建構子 "this.AcceptButton = this.btnConfirm"
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Click Event 訂閱在建構子 "this.CancelButton = this.btnCancel"
        }

        private void cmbTestSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateUIComponent();
        }

        #endregion
    }

    internal class FunctionType
    {
        private EMsrtType _type;
        private string _name;

        public FunctionType(EMsrtType type, string name)
        {
            this._type = type;
            this._name = name;
        }

        public EMsrtType MsrtType
        {
            get { return this._type; }
            set { this._type = value; }
        }

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

    }
}
