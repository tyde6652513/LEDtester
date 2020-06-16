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
    public partial class frmInfiniLED : Form
    {
        private string _lotNumber = string.Empty;
        private string _waferNumber = string.Empty;
        private string _productType = string.Empty;
        private string _cassetteNumber = string.Empty;
        private string _slotNumber = string.Empty;

        public frmInfiniLED()
        {
            InitializeComponent();

            Host.OnTestItemChangeEvent += new EventHandler(this.UpdateDataToUI);

            Host.OnBarcCodeSaveEvent += new EventHandler(this.SaveData);
        }

        private void frmBarcodeSetting_Load(object sender, EventArgs e)
        {

            UpdateDataToUI(null, null);
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

            DataCenter._uiSetting.CassetteNumber = this.txtCassetteNumber.Text;

            Host._MPIStorage.GenerateOutputFileName();

            DataCenter.SaveProductFile();

            UpdateDataToUI(null, null);
        }

        private void UpdateDataToUI(object o, EventArgs e)
        {
            // InfiniLED Request
            //DataCenter._uiSetting.LotNumber = this._lotNumber;
            //DataCenter._uiSetting.WaferNumber = this._waferNumber;
            //DataCenter._uiSetting.ProductType = this._productType;
            //DataCenter._uiSetting.CassetteNumber = this.txtCassetteNumber.Text;
            //DataCenter._uiSetting.SlotNumber = this._slotNumber;

            // UpdateToUI
            this.txtOutputFileName.Text = DataCenter._uiSetting.TestResultFileName;
            this.txtBarcode.Text = DataCenter._uiSetting.Barcode;  // form Prober
            this.txtLotNumber.Text = DataCenter._uiSetting.LotNumber;
            this.txtWaferNumber.Text = DataCenter._uiSetting.WaferNumber;
            this.txtProductType.Text = DataCenter._uiSetting.ProductType;
            this.txtOperatorName.Text = DataCenter._uiSetting.OperatorName;   // form Prober
            this.txtCassetteNumber.Text = DataCenter._uiSetting.CassetteNumber;
            this.txtSlotNumber.Text = DataCenter._uiSetting.SlotNumber;

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //this._lotNumber = this.txtLotNumber.Text;

            //this._waferNumber = this.txtWaferNumber.Text;

            //this._productType = this.txtProductType.Text;

            //this._cassetteNumber = this.txtCassetteNumber.Text;

            //this._slotNumber = this.txtSlotNumber.Text;

            this.SaveData(null, null);
        }
    }
}
