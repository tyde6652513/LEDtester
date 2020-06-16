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
    public partial class frmOpticalSwitchComp : Form
    {
        int _sysCh = 1;
        public frmOpticalSwitchComp()
        {
            InitializeComponent();
            richTextBox1.Clear();
        }
        public void SetSysChannel(int ch)
        {
            _sysCh = ch;
        }

        public void SetString(string str)
        {
            richTextBox1.Text = str;
        }

        private void btnSwitchThisCh_Click(object sender, EventArgs e)
        {
            if (AppSystem.Switch2OpticalCh(_sysCh))
            {
                btnSwitchThisCh.Text = "Selected !";
            }
        }
    }
}
