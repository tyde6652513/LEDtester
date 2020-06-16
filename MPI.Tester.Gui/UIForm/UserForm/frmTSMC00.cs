using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MPI.Tester.Data;
using System.IO;

namespace MPI.Tester.Gui
{
	public partial class frmTSMC00 : System.Windows.Forms.Form
	{
		private frmSetTaskSheet _frmSetTaskSheet = new frmSetTaskSheet();

		private string _uiFormDataFileName;

		private UIFormData _uiFormData;

		public frmTSMC00()
		{
			InitializeComponent();
		}

		private void frmTSMC00_Load(object sender, EventArgs e)
		{
			this._uiFormDataFileName = "UIFormData.xml";

			this.LoadUIFormData();

			this.LoadData(null, null);

            Host.OnTestItemChangeEvent += new EventHandler(this.LoadData);

            Host.OnBarcCodeSaveEvent += new EventHandler(this.SaveData);

			FormAgent.RecipeForm.OpRecipeSaveEvent += new EventHandler<EventArgs>(this.SaveData);

			FormAgent.RecipeForm.UpDateUserIDFormEvent += new EventHandler<EventArgs>(this.LoadData);
		}

		private void LoadData(object sender, EventArgs e)
		{
			if (!this.Created)
				return;

			//Board: Barcode, Array: Remark1, Stage: Remark2
			this.txtOutputFileName.Text = DataCenter._uiSetting.TestResultFileName;

			this.txtLotNumber.Text = DataCenter._uiSetting.LotNumber;

			this.txtBoardID.Text = DataCenter._uiSetting.WaferNumber;

			this.UpdateStage();

			this.txtArray.Text = DataCenter._uiSetting.WeiminUIData.Remark01;

			//this.txtStage.Text = DataCenter._uiSetting.WeiminUIData.Remark02;

			this.txtOperatorName.Text = DataCenter._uiSetting.OperatorName;

			switch (DataCenter._uiSetting.AuthorityLevel)
			{
				case EAuthority.Operator:
				case EAuthority.QC:
					this.gplEngineer.Visible = false;
					break;
				//-------------------------------------------------------------------
				case EAuthority.Engineer:
				case EAuthority.Super:
					if (DataCenter._product.TestResultPathByTaskSheet != string.Empty)
					{
						DataCenter.CreateTaskSheet(DataCenter._uiSetting.TaskSheetFileName);
					}
					this.gplEngineer.Visible = true;
					break;
				//-------------------------------------------------------------------
				default:
					this.gplEngineer.Visible = true;
					break;
			}
		}

		private void SaveData(object o, EventArgs e)
		{
			if (!this.Created)
			{
				return;
			}

			DataCenter._uiSetting.TestResultFileName = this.txtOutputFileName.Text;

			DataCenter._uiSetting.LotNumber = this.txtLotNumber.Text;

			DataCenter._uiSetting.WaferNumber = this.txtBoardID.Text;

			DataCenter._uiSetting.WeiminUIData.Remark01 = this.txtArray.Text;

			DataCenter._uiSetting.WeiminUIData.Remark02 = this.cmbStage.Text;

			DataCenter._uiSetting.OperatorName = this.txtOperatorName.Text;

		}

		private void LoadUIFormData()
		{
			string path = Path.Combine(Constants.Paths.DATA_FILE, this._uiFormDataFileName);

			if (!File.Exists(path))
			{
				this._uiFormData = new UIFormData();

				return;
			}

			try
			{
				this._uiFormData = MPI.Xml.XmlFileSerializer.Deserialize(typeof(UIFormData), path) as UIFormData;
			}
			catch
			{
			}
		}

		private void SaveUIFormData()
		{
			string path = Path.Combine(Constants.Paths.DATA_FILE);

			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			try
			{
				MPI.Xml.XmlFileSerializer.Serialize(this._uiFormData, Path.Combine(path, this._uiFormDataFileName));
			}
			catch
			{
			}
		}

		private void UpdateStage()
		{
			this.cmbStage.Items.Clear();

			this.cmbStage.Items.AddRange(this._uiFormData.Stage.ToArray());

			this.cmbStage.SelectedItem = DataCenter._uiSetting.WeiminUIData.Remark02;

			if (this.cmbStage.SelectedItem == null && this.cmbStage.Items.Count > 0)
			{
				this.cmbStage.SelectedIndex = 0;
			}


			this.cmbStageEdit.Items.Clear();

			this.cmbStageEdit.Items.AddRange(this._uiFormData.Stage.ToArray());

			this.cmbStageEdit.SelectedItem = DataCenter._uiSetting.WeiminUIData.Remark02;

			if (this.cmbStageEdit.Items.Count > 0)
			{
				this.cmbStageEdit.SelectedIndex = 0;
			}
		}

		private void btnAddStage_Click(object sender, EventArgs e)
		{
			if (this.txtStage.Text == "")
				return;

			if (this._uiFormData.Stage.Contains(this.txtStage.Text))
			{
				this.txtStage.Text = "";

				return;
			}

			this._uiFormData.Stage.Add(this.txtStage.Text);

			this.txtStage.Text = "";

			this.UpdateStage();

			this.SaveUIFormData();
		}

		private void btnRemoveStage_Click(object sender, EventArgs e)
		{
			if (!this._uiFormData.Stage.Contains(this.cmbStageEdit.SelectedItem))
				return;

			this._uiFormData.Stage.Remove(this.cmbStageEdit.SelectedItem as string);

			this.UpdateStage();

			this.SaveUIFormData();
		}

		private void btnNewByBinTable_Click(object sender, EventArgs e)
		{
			UILog.Log(this, sender, "btnNewByBinTable_Click");

			DataCenter._uiSetting.ControlTaskSettingUI = EControlTaskSetting.TSMC;

			this._frmSetTaskSheet.ShowDialog();

			if (DataCenter._uiSetting.SendTaskFileName == "")
				return;

			string fileName = DataCenter._uiSetting.SendTaskFileName;

			if (File.Exists(Path.Combine(DataCenter._uiSetting.ProductPath, fileName + "." + Constants.Files.TASK_SHEET_EXTENSION)))
			{
				Host.SetErrorCode(EErrorCode.TaskSheetFileExisted);

				return;
			}

			ELOPSaveItem lopSaveItem = DataCenter._product.LOPSaveItem;

			DataCenter._uiSetting.TaskSheetFileName = fileName;

			DataCenter.CreateTaskSheet(fileName, false);

			// Load Bin Data File
			string path = Constants.Paths.PRODUCT_FILE02;

			string fileNameWithExt = DataCenter._uiSetting.BinDataFileName + "." + Constants.Files.BIN_FILE_EXTENSION;

            if (DataCenter._smartBinning.Load(fileName))
            {
                DataCenter.SaveBinFile();
            }

			DataCenter.CreateProductFile(fileName);

			DataCenter._product.LOPSaveItem = lopSaveItem;

			DataCenter.Save();

			Host.UpdateDataToAllUIForm();
		}

		[Serializable]
		public class UIFormData
		{
			private object _lockObj;

			private List<string> _stage;

			public UIFormData()
			{
				this._lockObj = new object();
				this._stage = new List<string>();
			}

			public List<string> Stage
			{
				get { return this._stage; }
				set { lock (this._lockObj) { this._stage = value; } }
			}
		}
	}
}
