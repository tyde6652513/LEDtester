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

namespace MPI.Tester.Gui
{

    public partial class frmOsaCoupling : Form
    {

        private bool _isSmuInit;
        private bool _isOsaInit;

        public frmOsaCoupling()
        {
            InitializeComponent();

            this._isSmuInit = false;
            this._isOsaInit = false;
        }

        public frmOsaCoupling(MachineInfoData info) : this()
        {
            this._isSmuInit = info.IsSrcInitSuccess;
            this._isOsaInit = info.IsOsaInitSuccess;

            this.grpSmuSetting.Enabled = this._isSmuInit;
            this.grpOsaSetting.Enabled = this._isOsaInit;
        }

        #region >>> Private Method <<<

        private void frmOsaCoupling_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._isSmuInit)
            {
                AppSystem.RunSmuOutput(0.0d, 0.0d, false);
            }

            if (this._isOsaInit)
            {
                AppSystem.RunOsaCoupling(false);
            }
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void btnSmuOutputON_Click(object sender, EventArgs e)
        {
            double srcI = this.dinForceValue.Value / 1000.0d;  // mA -> A
            double msrtV = this.dinMsrtClamp.Value;

            AppSystem.RunSmuOutput(srcI, msrtV, true);
        }

        private void btnSmuOutputOFF_Click(object sender, EventArgs e)
        {
            AppSystem.RunSmuOutput(0.0d, 0.0d, false);
        }

        private void btnOsaRepeat_Click(object sender, EventArgs e)
        {
            AppSystem.RunOsaCoupling(true);
        }

        private void btnOsaStop_Click(object sender, EventArgs e)
        {
            AppSystem.RunOsaCoupling(false);
        }

        #endregion


    }
}
