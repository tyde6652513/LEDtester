using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MPI.Tester.Gui
{
    public partial class frmReportComments : Form
    {
        public frmReportComments()
        {
            InitializeComponent();
        }

        private void frmReportComments_Load(object sender, EventArgs e)
        {
            this.txtReportComments.Text = string.Empty;

            this.txtReportComments.MaxLength = 100;

            this.TopMost = true;

            this.TopLevel = true;

            this.Focus();

            this.TopMost = false;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DataCenter._uiSetting.ReportComments = this.txtReportComments.Text;

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
