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

     public enum EMessageType
     {
         Question=1,
         Warning=2,
     }



    public partial class frmAlarmMessage : Form
    {
        private DialogResult _result;

        public frmAlarmMessage()
        {
            InitializeComponent();
        }

        private void frmAlarmMessage_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this._result = DialogResult.Cancel;
        }

        public string Title
        {
            set
            {
                lblTitle.Text = value;
            }
        }

        public string Message
        {
            set
            {
                txtMessage.Text = value;
            }
        }

        public EMessageType MessageType
        {
            set
            {
                if (value == EMessageType.Question)
                {
                    lblTitle.ForeColor = Color.Blue;

                    lblTitle.Text = "QUESTION";

                    this.txtMessage.ForeColor = Color.Black;
                }
                else if (value == EMessageType.Warning)
                {
                    lblTitle.ForeColor = Color.Red;

                    lblTitle.Text = "WARNING";

                    this.txtMessage.ForeColor = Color.Red;
                }
            }
        }

        public string ErrorHandler
        {
            set
            {
              
                this.lblErrorHandler.Text = value;
            }
        }

        public DialogResult Result
        {
            get
            {
                return this._result;
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this._result = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this._result = DialogResult.Cancel;
            this.Close();
        }

        private void frmAlarmMessage_Closd(object sender, FormClosedEventArgs e)
        {
          //this._result = DialogResult.Cancel;
            this.Close();
        }

    }
}
