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
    public partial class frmProgressBar : Form
    {
        public frmProgressBar()
        {
            InitializeComponent();
        }

        public int ProgressBarValue
        {
            get { return this.progressBar.Value; }
            set 
            {
                this.progressBar.Value = value;
                this.Update();
            }
        }

        public int ProgressBarMax
        {
            get { return this.progressBar.Maximum; }
            set { this.progressBar.Maximum = value; }
        }

        public void Start( int maximun, string title)
        {
			this.Show();
            this.progressBar.Minimum = 0;
			this.progressBar.Maximum = maximun;
            this.lblTitle.Text = title;
            this.Update();
        }

        private void frmProgressBar_Load(object sender, EventArgs e)
        {
            System.Drawing.Point location = new System.Drawing.Point();
            location.X = FormAgent.BaseForm.Location.X + FormAgent.BaseForm.Width / 2 - this.Width / 2;
            location.Y = FormAgent.BaseForm.Location.Y + FormAgent.BaseForm.Height / 2 - this.Height / 2;
            this.Location = location;
        }
    }
}
