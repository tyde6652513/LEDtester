using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
    public partial class frmDailyWatch : Form
    {
        public frmDailyWatch()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSetTaskSheet_Load(object sender, EventArgs e)
        {
            //if (DataCenter._uiSetting.UserDefinedData.CmUIControl == 1)
            //{
            //    this.plSptXaxisCoeff.Visible = true;
            //    this.UpdateSptInformation();
            //}
            //else
            //{
            //    this.plSptXaxisCoeff.Visible = false;
            //}
        }

        private void UpdateSptInformation()
        {
            this.dgvSptFactor.Rows.Clear();
            this.lblIntecept.Text = "";
            this.lblFirstCoefficient.Text = "";
            this.lblSecondCoefficient.Text = "";
            this.lblThirdCoefficient.Text = "";
            this.txtYCalibFilePath.Text = DataCenter._product.SptCalibPathAndFile;

            if (DataCenter._product.ProductSptXwaveCoef == null ||
                DataCenter._product.ProductSptYintCoef == null)
            {
                return;
            }
            double[] tempWavelength = new double[2048];
            this.lblIntecept.Text = DataCenter._product.ProductSptXwaveCoef[0].ToString();
            this.lblFirstCoefficient.Text = DataCenter._product.ProductSptXwaveCoef[1].ToString();
            this.lblSecondCoefficient.Text = DataCenter._product.ProductSptXwaveCoef[2].ToString();
            this.lblThirdCoefficient.Text = DataCenter._product.ProductSptXwaveCoef[3].ToString();

            for (int i = 0; i < 2048; i++)
            {
                this.dgvSptFactor.Rows.Add();
                this.dgvSptFactor.Rows[i].Cells[0].Value = i.ToString();
                tempWavelength[i] = DataCenter._product.ProductSptXwaveCoef[0] + (DataCenter._product.ProductSptXwaveCoef[1] * i) + DataCenter._product.ProductSptXwaveCoef[2] * Math.Pow(i, 2) + DataCenter._product.ProductSptXwaveCoef[3] * Math.Pow(i, 3);
                this.dgvSptFactor.Rows[i].Cells[1].Value = tempWavelength[i].ToString();
                this.dgvSptFactor.Rows[i].Cells[2].Value = DataCenter._product.ProductSptYintCoef[i].ToString();
                this.dgvSptFactor.Rows[i].Cells[3].Value = DataCenter._product.ProductSptYweight[i].ToString();
            }
        }

        private void btnAddBinItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
