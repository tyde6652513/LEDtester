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


namespace MPI.Tester.Gui.UIForm.UserForm.Condition
{
    public partial class frmWAVETEC00Condition : Form ,IfrmCusConditin
    {
        List<string> esNameList = new List<string>();

        bool _isRefreshEnd = true;
        public frmWAVETEC00Condition()
        {
            InitializeComponent();
            esNameList.Add("NoneSelect");

            txtProductType.Text = DataCenter._product.ProductName;

            LoadESNames();

        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            Save();
            DataCenter.SaveProductFile();
            Refresh();
        }

        private void dgvResultName_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (this.dgvResultName.Rows != null &&
            //    e.RowIndex >= 0 && e.RowIndex < this.dgvResultName.Rows.Count)
            //{
            //    if (!_isRefreshEnd || this.dgvResultName.Rows[e.RowIndex].IsNewRow)
            //    {
            //        this.dgvResultName[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Empty;
            //    }
            //    else
            //    {
            //        this.dgvResultName[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.DarkOrange;
            //    }
            //}
        }

        #region
        public bool Save()
        {
            DataCenter._uiSetting.EdgeSensorName = cmbEdgeSensor.Text;

            DataCenter._product.ProductName = txtProductType.Text;
            return true;
        }

        public bool Refresh()
        {
            _isRefreshEnd = false;

            txtProductType.Text = DataCenter._product.ProductName;
            _isRefreshEnd = true;
            return true;
        }

        #endregion

        #region>>private method<<

        private void LoadESNames()
        {
            string tarFolder = Constants.Paths.ROOT;
            string tarFile = Path.Combine(tarFolder, "EdgeSensor.dat");

            esNameList.Clear();
            esNameList.Add("NoneSelect");
            if (File.Exists(tarFile))
            {
                using (StreamReader sr = new StreamReader(tarFile))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine().Trim();
                        if (line != "")
                        {
                            esNameList.Add(line);
                        }
                    }
                }
            }
            cmbEdgeSensor.Items.Clear();
            cmbEdgeSensor.Items.AddRange(esNameList.ToArray());
            cmbEdgeSensor.SelectedIndex = 0;
        }

        #endregion
        
    }
}
