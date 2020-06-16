using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public partial class frmKAISTAR : System.Windows.Forms.Form
	{
		public frmKAISTAR()
		{
			InitializeComponent();
		}

		private void frmKAISTAR_Load(object sender, EventArgs e)
		{
			this.LoadData(null, null);

			FormAgent.RecipeForm.OpRecipeSaveEvent += new EventHandler<EventArgs>(this.SaveData);

			FormAgent.RecipeForm.UpDateUserIDFormEvent += new EventHandler<EventArgs>(this.LoadData);
		}

		private void LoadData(object sender, EventArgs e)
		{
			this.txtTestResultPath.Text = DataCenter._uiSetting.TestResultPath01;

			this.txtOutputFileName.Text = DataCenter._uiSetting.TestResultFileName;

			this.txtProductType.Text = DataCenter._uiSetting.ProductType;

			this.txtLotNumber.Text = DataCenter._uiSetting.LotNumber;

			this.txtSubstrate.Text = DataCenter._uiSetting.Substrate;

			this.txtOperatorName.Text = DataCenter._uiSetting.OperatorName;
		}

		private void SaveData(object o, EventArgs e)
		{
			DataCenter._uiSetting.TestResultPath01 = this.txtTestResultPath.Text;

			DataCenter._uiSetting.TestResultFileName = this.txtOutputFileName.Text;

			DataCenter._uiSetting.ProductType = this.txtProductType.Text;

			DataCenter._uiSetting.LotNumber = this.txtLotNumber.Text;

			DataCenter._uiSetting.Substrate = this.txtSubstrate.Text;

			DataCenter._uiSetting.OperatorName = this.txtOperatorName.Text;

		}
	}
}
