using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Reflection;
using MPI.Tester.Data;

namespace MPI.Tester.Gui
{
	public partial class frmHelio : System.Windows.Forms.Form
	{
		enum DateType
		{
			Month = 0,
			Year = 1
		}

		public frmHelio()
		{
			InitializeComponent();
		}

		private void frmHelio_Load(object sender, EventArgs e)
		{
			DirectoryInfo di = new DirectoryInfo(Constants.Paths.BARCODE_PRINT_DIR);

			if (di.Exists)
			{
				string key = "PrintFormat" + ((int)DataCenter._uiSetting.UserID).ToString();

				FileInfo[] fileInfos = di.GetFiles(key + "*");

				string[] strS = new string[fileInfos.Length];

				for (int i = 0; i < strS.Length; i++)
				{
					strS[i] = fileInfos[i].Name.Replace(key, "Format");

					strS[i] = strS[i].Replace(".xslt", "");
				}

				this.cmbBarcodePrintFormat.Items.AddRange(strS);
			}

			this.LoadData(null, null);

			FormAgent.RecipeForm.OpRecipeSaveEvent += new EventHandler<EventArgs>(this.SaveData);

			FormAgent.RecipeForm.UpDateUserIDFormEvent += new EventHandler<EventArgs>(this.LoadData);
		}

		private void LoadData(object sender, EventArgs e)
		{
			//LotNumber
			this.cmbYear.Items.Clear();

			for (int i = 2012; i < 2035; i++)
			{
				this.cmbYear.Items.Add(i.ToString());
			}

			this.cmbMonth.Items.Clear();
			
			for (int i = 1; i < 13; i++)
			{
				if (i < 10)
				{
					this.cmbMonth.Items.Add("0" + i.ToString());
				}
				else
				{
					this.cmbMonth.Items.Add(i.ToString());
				}
			}

			this.txtLotNumber1.Text = DataCenter._uiSetting.WeiminUIData.Remark01;

			this.cmbYear.SelectedItem = this.NumberToDate(DataCenter._uiSetting.WeiminUIData.Remark02, DateType.Year);
			
			this.cmbMonth.SelectedItem = this.NumberToDate(DataCenter._uiSetting.WeiminUIData.Remark03, DateType.Month);
			
			this.txtLotNumber2.Text = DataCenter._uiSetting.WeiminUIData.Remark04;

			if (this.cmbYear.SelectedItem == null)
			{
				this.cmbYear.SelectedIndex = 0;
			}

			if (this.cmbMonth.SelectedItem == null)
			{
				this.cmbMonth.SelectedIndex = 0;
			}

			//英文字M代表製令單、A代表託工單、R代表重工、D代表研發工單。
			this.comboBoxProductTypeID.Items.Clear();

			this.comboBoxProductTypeID.Items.Add("M");

			this.comboBoxProductTypeID.Items.Add("A");

			this.comboBoxProductTypeID.Items.Add("R");

			this.comboBoxProductTypeID.Items.Add("D");

			int index = -1;

			if (DataCenter._uiSetting.ProductType.Length > 0)
			{
				string TaskID = DataCenter._uiSetting.ProductType.Remove(1, (DataCenter._uiSetting.ProductType.Length - 1));

				index = this.comboBoxProductTypeID.Items.IndexOf(TaskID);

				if (index > 0)
				{
					this.comboBoxProductTypeID.SelectedIndex = index;
				}
				else
				{
					this.comboBoxProductTypeID.SelectedIndex = 0;
				}

				this.txtProductType.Text = DataCenter._uiSetting.ProductType.Remove(0, 1);
			}

			this.txtTestResultPath.Text = DataCenter._uiSetting.TestResultPath01;

			this.txtOutputFileName.Text = DataCenter._uiSetting.TestResultFileName;

			//this.txtLotNumber.Text = DataCenter._uiSetting.LotNumber;

			this.txtProductName.Text = DataCenter._uiSetting.ProductName;

			this.txtBarcode.Text = DataCenter._uiSetting.Barcode;

			this.txtOperatorName.Text = DataCenter._uiSetting.OperatorName;

			if (!this.cmbBarcodePrintFormat.Items.Contains(DataCenter._product.BarcodePrintFormat))
			{
				this.cmbBarcodePrintFormat.SelectedIndex = -1;
			}
			else
			{
				this.cmbBarcodePrintFormat.SelectedItem = DataCenter._product.BarcodePrintFormat;
			}
		}

		private void SaveData(object o, EventArgs e)
		{
			DataCenter._uiSetting.TestResultPath01 = this.txtTestResultPath.Text;

			DataCenter._uiSetting.TestResultFileName = this.txtOutputFileName.Text;

			DataCenter._uiSetting.WeiminUIData.Remark01 = this.txtLotNumber1.Text;

			DataCenter._uiSetting.WeiminUIData.Remark02 = this.DateToNumber(int.Parse(this.cmbYear.SelectedItem.ToString()), DateType.Year);

			DataCenter._uiSetting.WeiminUIData.Remark03 = this.DateToNumber(int.Parse(this.cmbMonth.SelectedItem.ToString()), DateType.Month);

			DataCenter._uiSetting.WeiminUIData.Remark04 = this.txtLotNumber2.Text;

			DataCenter._uiSetting.LotNumber = this.txtLotNumber1.Text + "-" + this.cmbYear.SelectedItem.ToString() + this.cmbMonth.SelectedItem.ToString() + this.txtLotNumber2.Text;

			//DataCenter._uiSetting.LotNumber = this.txtLotNumber.Text;

			DataCenter._uiSetting.ProductName = this.txtProductName.Text;

			int index = this.comboBoxProductTypeID.SelectedIndex;

			DataCenter._uiSetting.ProductType = this.comboBoxProductTypeID.Items[index] + this.txtProductType.Text;

			DataCenter._uiSetting.Barcode = this.txtBarcode.Text;

			DataCenter._uiSetting.OperatorName = this.txtOperatorName.Text;

			if (this.cmbBarcodePrintFormat.SelectedItem == null)
			{
				DataCenter._product.BarcodePrintFormat = string.Empty;
			}
			else
			{
				DataCenter._product.BarcodePrintFormat = this.cmbBarcodePrintFormat.SelectedItem.ToString();
			}

			DataCenter.SaveProductFile();
		}

		private string NumberToDate(string number, DateType dateType)
		{
			//Number Range: from A to Z
			int index = 0;
			string date = string.Empty;
			string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

			if (str.Contains(number.ToString().ToUpper()))
			{
				index = str.IndexOf(number.ToString().ToUpper());
			}
			else if (dateType == DateType.Month)
			{
				index = 10;
			}
			else if (dateType == DateType.Year)
			{
				index = 12;
			}

			switch (dateType)
			{
				case DateType.Month:
					date = index.ToString("00");
					break;
				case DateType.Year:
					date = (2000 + index).ToString("0000");
					break;
				default:
					break;
			}

			return date;
		}

		private string DateToNumber(int date, DateType dateType)
		{
			//Date Range:
			//Year:  from 2008 to 2035
			//Month: from 1 to 12
			int index = 0;
			string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

			switch (dateType)
			{
				case DateType.Month:
					if (date < 1 || date > 12)
						index = 10;
					else
						index = date;
					break;
				case DateType.Year:
					if (date < 2009 || date > 2035)
						index = 9;
					else
						index = date - 2000;
					break;
			}

			return str[index].ToString();
		}
	}
}
