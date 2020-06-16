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
	public partial class frmDefault : System.Windows.Forms.Form
	{
		public frmDefault()
		{
			InitializeComponent();
		}

		private void frmDefault_Load(object sender, EventArgs e)
		{
			this.LoadData(null, null);

			FormAgent.RecipeForm.OpRecipeSaveEvent += new EventHandler<EventArgs>(this.SaveData);

			FormAgent.RecipeForm.UpDateUserIDFormEvent += new EventHandler<EventArgs>(this.LoadData);
		}

		private void LoadData(object sender, EventArgs e)
		{
			this.txtTestResultPath.Text = DataCenter._uiSetting.TestResultPath01;
			this.txtOutputFileName.Text = DataCenter._uiSetting.TestResultFileName;
			this.txtProductName.Text = DataCenter._uiSetting.ProductName;
			this.txtBarcode.Text = DataCenter._uiSetting.Barcode;
			this.txtLotNumber.Text = DataCenter._uiSetting.LotNumber;
			this.txtWaferNumber.Text = DataCenter._uiSetting.WaferNumber;
			this.txtOperatorName.Text = DataCenter._uiSetting.OperatorName;
		}

		private void SaveData(object o, EventArgs e)
		{
			DataCenter._uiSetting.TestResultPath01 = this.txtTestResultPath.Text;

			DataCenter._uiSetting.TestResultFileName = this.txtOutputFileName.Text;

			DataCenter._uiSetting.LotNumber = this.txtLotNumber.Text;

			DataCenter._uiSetting.ProductName = this.txtProductName.Text;

			DataCenter._uiSetting.WaferNumber = this.txtWaferNumber.Text;

			DataCenter._uiSetting.Barcode = this.txtBarcode.Text;

			DataCenter._uiSetting.OperatorName = this.txtOperatorName.Text;

		}
	}
}
