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
    public partial class frmBarcodeSetting : Form
    {
        public frmBarcodeSetting()
        {
            InitializeComponent();

            Host.OnTestItemChangeEvent += new EventHandler(this.UpdateDataToUI);

            Host.OnBarcCodeSaveEvent += new EventHandler(this.SaveData);
        }

        private void frmBarcodeSetting_Load(object sender, EventArgs e)
        {
            UpdateDataToUI(null,null);
        }

        private void SaveData(object o, EventArgs e)
        {
            DataCenter._uiSetting.TestResultFileName = this.txtOutputFileName.Text;

            DataCenter._uiSetting.LotNumber = this.txtLotNumber.Text;

            DataCenter._uiSetting.WaferNumber = this.txtWaferNumber.Text;

            DataCenter._uiSetting.Barcode = this.txtBarcode.Text;

            DataCenter._uiSetting.OperatorName = this.txtOperatorName.Text;

            DataCenter._uiSetting.ProductType = this.txtProductType.Text;

            DataCenter._uiSetting.SlotNumber = this.txtSlotNumber.Text;

            Host._MPIStorage.GenerateOutputFileName();

            DataCenter.SaveProductFile();

            UpdateDataToUI(null, null);
        }

        private void UpdateDataToUI(object o, EventArgs e)
        {
            this.txtOutputFileName.Text = DataCenter._uiSetting.TestResultFileName;
            this.txtBarcode.Text = DataCenter._uiSetting.Barcode;
            this.txtLotNumber.Text = DataCenter._uiSetting.LotNumber;
            this.txtWaferNumber.Text = DataCenter._uiSetting.WaferNumber;
            this.txtProductType.Text = DataCenter._uiSetting.ProductType;
            this.txtOperatorName.Text = DataCenter._uiSetting.OperatorName;

            this.txtCassetteNumber.Text = DataCenter._uiSetting.CassetteNumber;
            this.txtSlotNumber.Text = DataCenter._uiSetting.SlotNumber;

        }

    }
}
