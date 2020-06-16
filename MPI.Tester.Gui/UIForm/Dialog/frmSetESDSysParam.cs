using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MPI.Tester.Data;
using MPI.Tester.DeviceCommon;

namespace MPI.Tester.Gui
{
    public partial class frmSetESDSysParam : System.Windows.Forms.Form
    {
        private const string CALI_FILE_PATH = @"C:\MPI\LEDTester\ESD";
        
        public ESDGainTable _dataTable;

        private string _pathWithExt;

        public frmSetESDSysParam()
        {
            InitializeComponent();

            string fileNameWithExt = "ESDSysParam.dat";

            this._pathWithExt = Path.Combine(CALI_FILE_PATH, fileNameWithExt);

            this.InitUIComponent();
        }

        #region >>> Private Method <<<

        private void InitUIComponent()
        {
            this._dataTable = (ESDGainTable)Deserialize<ESDGainTable>(this._pathWithExt);

            if (this._dataTable == null)
            {
                this._dataTable = new ESDGainTable();
            }

            this.UpdateDataToDgv();
        }

        private void UpdateDataToDgv()
        {
            this.dgvESDHBM.Rows.Clear();
            this.dgvESDMM.Rows.Clear();

            //ESDGainTable dataTable = DataCenter._sysSetting.ESDSystemGain;

            if (this._dataTable == null)
            {
                this.dgvESDHBM.Update();
                this.dgvESDHBM.Update();

                return;
            }

            this.dgvESDHBM.SuspendLayout();
            this.dgvESDMM.SuspendLayout();

            this.dgvESDHBM.Columns[0].DefaultCellStyle.BackColor = Color.YellowGreen;

            for (int i = 0; i < this._dataTable.HBM.Length; i++)
            {
                this.dgvESDHBM.Rows.Add();

                this.dgvESDHBM[0, i].Value = (i + 1).ToString();
                this.dgvESDHBM[1, i].Value = this._dataTable.HBM[i].LowerBoundary;
                this.dgvESDHBM[2, i].Value = this._dataTable.HBM[i].UpperBoundary;
                this.dgvESDHBM[3, i].Value = this._dataTable.HBM[i].Gain;
            }

            this.dgvESDMM.Columns[0].DefaultCellStyle.BackColor = Color.YellowGreen;

            for (int i = 0; i < this._dataTable.MM.Length; i++)
            {
                this.dgvESDMM.Rows.Add();

                this.dgvESDMM[0, i].Value = (i + 1).ToString();
                this.dgvESDMM[1, i].Value = this._dataTable.MM[i].LowerBoundary;
                this.dgvESDMM[2, i].Value = this._dataTable.MM[i].UpperBoundary;
                this.dgvESDMM[3, i].Value = this._dataTable.MM[i].Gain;
            }

            this.dgvESDHBM.ResumeLayout();
            this.dgvESDMM.ResumeLayout();
        }

        private static bool Serialize(string FileName, object Obj)
        {
            try
            {
                if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
                {
                    System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(Obj.GetType());
                    System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(FileName, Encoding.ASCII);
                    x.Serialize(xmlTextWriter, Obj);
                    xmlTextWriter.Close();
                }
                else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Create))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        binaryFormatter.Serialize(fileStream, Obj);

                        fileStream.Close();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static T Deserialize<T>(string FileName)
        {
            System.Xml.XmlDocument xdoc = new System.Xml.XmlDocument();

            try
            {
                object obj = new object();

                if (System.IO.Path.GetExtension(FileName).ToLower() == ".xml")
                {
                    xdoc.Load(FileName);
                    System.Xml.XmlNodeReader reader = new System.Xml.XmlNodeReader(xdoc.DocumentElement);
                    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    obj = ser.Deserialize(reader);
                }
                else if (System.IO.Path.GetExtension(FileName).ToLower() == ".dat")
                {
                    using (System.IO.FileStream fileStream = new System.IO.FileStream(FileName, System.IO.FileMode.Open))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        obj = binaryFormatter.Deserialize(fileStream);

                        fileStream.Close();
                    }
                }

                return (T)obj;
            }
            catch
            {
                return default(T);
            }
        }

        #endregion

        #region >>> Public Method <<<

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this._dataTable == null)
            {
                return;
            }

            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.CheckIsReStartSystem, "System Setting , Please Restart the application？", "Close", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.OK)
            {
                return;
            }

            //--------------------------------------------------------------------
            // Save ESD HBM Gain Data
            //--------------------------------------------------------------------
            for (int i = 0; i < this.dgvESDHBM.Rows.Count; i++)
            {
                this._dataTable.HBM[i].LowerBoundary = Convert.ToInt32(this.dgvESDHBM[1, i].Value.ToString());
                this._dataTable.HBM[i].UpperBoundary = Convert.ToInt32(this.dgvESDHBM[2, i].Value.ToString());
                this._dataTable.HBM[i].Gain = Convert.ToDouble(this.dgvESDHBM[3, i].Value.ToString());
            }

            //--------------------------------------------------------------------
            // Save ESD MM Gain Data
            //--------------------------------------------------------------------
            for (int i = 0; i < this.dgvESDMM.Rows.Count; i++)
            {
                this._dataTable.MM[i].LowerBoundary = Convert.ToInt32(this.dgvESDMM[1, i].Value.ToString());
                this._dataTable.MM[i].UpperBoundary = Convert.ToInt32(this.dgvESDMM[2, i].Value.ToString());
                this._dataTable.MM[i].Gain = Convert.ToDouble(this.dgvESDMM[3, i].Value.ToString());
            }

            Serialize(this._pathWithExt, this._dataTable);

            FormAgent.MainForm.IsClose = true;

            FormAgent.MainForm.Close();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.InitUIComponent();
        }

        private void btnRestESDGain_Click(object sender, EventArgs e)
        {
            DialogResult result = TopMessageBox.ShowMessage((int)EMessageCode.CheckIsResetESDSystemParameter, "Would you Reset ESD Gain Table?", "Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result != DialogResult.OK)
                return;

            foreach (ESDGainData data in this._dataTable.HBM)
            {
                data.Gain = 1.0d;
            }

            foreach (ESDGainData data in this._dataTable.MM)
            {
                data.Gain = 1.0d;
            }

            this.UpdateDataToDgv();
        }

        #endregion
    }
}
