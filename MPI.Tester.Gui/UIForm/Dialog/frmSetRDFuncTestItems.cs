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
    public partial class frmSetRDFuncTestItems : Form
    {
        
        public frmSetRDFuncTestItems()
        {
            InitializeComponent();

            this.InitDgv();
        }

        #region >>> Private Method <<<

        private void InitDgv()
        {
            int rowCount = 0;
            
            List<string> testTypes = new List<string>(Enum.GetNames(typeof(ETestType)));

            this.dgvData.SuspendLayout();

            this.dgvData.Rows.Clear();

            foreach (var type in testTypes)
            {
                this.dgvData.Rows.Add();

                this.dgvData.Rows[rowCount].Cells["colNo"].Value = (rowCount + 1).ToString();

                this.dgvData.Rows[rowCount].Cells["colKeyName"].Value = type;

                rowCount++;
            }

            this.dgvData.ResumeLayout();
        }

        #endregion

        #region >>> Public Method <<<

        public void UpdateItems(string[] items)
        {
            if(items == null || items.Length == 0)
            {
                this.InitDgv();

                return;
            }

            for (int i = 0; i < this.dgvData.Rows.Count; i++)
            {
                for (int j = 0; j < items.Length; j++)
                {
                    if (this.dgvData.Rows[i].Cells["colKeyName"].Value.ToString() == items[j])
                    {
                        this.dgvData.Rows[i].Cells["colEnable"].Value = true;
                    }
                }
            }           
        }

        public string[] GetItems()
        {
            List<string> lstItems = new List<string>();

            for (int i = 0; i < this.dgvData.Rows.Count; i++)
            {
                if (Convert.ToBoolean(this.dgvData.Rows[i].Cells["colEnable"].Value) == true)
                {
                    lstItems.Add(this.dgvData.Rows[i].Cells["colKeyName"].Value.ToString());
                }
            }

            return lstItems.ToArray();
        }

        #endregion

        #region >>> UI Ctrl <<<

        private void frmSetRDFuncTestItems_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
