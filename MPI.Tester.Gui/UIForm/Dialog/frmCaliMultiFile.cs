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
    public partial class frmMultiFile : Form
    {
        private List<string> _stdFile = new List<string>();
        private List<string> _msrtFile = new List<string>();

        public frmMultiFile()
        {
            InitializeComponent();
        }

        private void btnLoadStdFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "CSV文件|*.CSV";
            openFileDialog1.InitialDirectory = DataCenter._toolConfig.StdFileDir;
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;
            // 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.dgvOpenFile.CurrentCell != null)
                {
                    if (this.dgvOpenFile.CurrentCell.RowIndex == 0 && this.dgvOpenFile.CurrentCell.ColumnIndex != 1) //2
                    {
                        this._stdFile.Add(openFileDialog1.FileName);
                    }
                    else
                    {
                        if (this.dgvOpenFile.CurrentCell.RowIndex >= this._stdFile.Count)
                        {
                            this._stdFile.Add(openFileDialog1.FileName);
                        }
                        else
                        {
                            this._stdFile[this.dgvOpenFile.CurrentCell.RowIndex] = openFileDialog1.FileName;
                        }
                    }
                }
                else
                {
                    this._stdFile.Add(openFileDialog1.FileName);
                }
                this.UpdateToUI();
            }
        }

        private void CaliMultiFile_Load(object sender, EventArgs e)
        {

        }

        private void btnOpenMutiMsrtFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "CSV文件|*.CSV";
            openFileDialog1.InitialDirectory = DataCenter._toolConfig.MsrtFileDir;
            openFileDialog1.FileName = "";
            openFileDialog1.FilterIndex = 1;    // default value = 1
            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (this.dgvOpenFile.CurrentCell != null)
                {
                    if (this.dgvOpenFile.CurrentCell.RowIndex == 0 && this.dgvOpenFile.CurrentCell.ColumnIndex != 2) //2
                    {
                        this._msrtFile.Add(openFileDialog1.FileName);
                    }
                    else
                    {
                        if (this.dgvOpenFile.CurrentCell.RowIndex >= this._msrtFile.Count)
                        {
                            this._msrtFile.Add(openFileDialog1.FileName);
                        }
                        else
                        {
                            this._msrtFile[this.dgvOpenFile.CurrentCell.RowIndex] = openFileDialog1.FileName;
                        }
                    }
                }
                else
                {
                    this._msrtFile.Add(openFileDialog1.FileName);
                }
                this.UpdateToUI();
            }
        }

        private void btnMultiFileOK_Click(object sender, EventArgs e)
        {
            if (this._stdFile.Count != this._msrtFile.Count)
            {
                DialogResult result = DevComponents.DotNetBar.MessageBoxEx.Show("Standard And Measurement Data Count Not  Match", "Error Msg", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.OK)
                    return;
            }

            if (this.LoadStdAndMsrtData() == true)
            {
                DataCenter._fileCompare.IsMultiFileMode = true;
            }
            else
            {
                DataCenter._fileCompare.IsMultiFileMode = false;
            }

            this.Hide();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnDeleteSingle_Click(object sender, EventArgs e)
        {
            if (this.dgvOpenFile.CurrentCell == null)
            {
                return;
            }

            switch (this.dgvOpenFile.CurrentCell.ColumnIndex)
            {
                case 1:
                    this._stdFile[this.dgvOpenFile.CurrentCell.ColumnIndex] = "";
                    break;
                case 2:
                    this._msrtFile[this.dgvOpenFile.CurrentCell.ColumnIndex] = "";
                    break;
                default:
                    break;
            }
            this.UpdateToUI();
        }

        private void btnDeleteAllFile_Click(object sender, EventArgs e)
        {
            this._msrtFile.Clear();
            this._stdFile.Clear();
            this.UpdateToUI();
        }

        private bool LoadStdAndMsrtData()
        {
            DataCenter._fileCompare.LoadStdDataCount = 0;
            DataCenter._fileCompare.LoadMsrtDataCount = 0;
            for (int i = 0; i < this._stdFile.Count; i++)
            {
                if (DataCenter._uiSetting.UserID == EUserID.Lextar)
                {
                    if (DataCenter._fileCompare.LoadStdFromFile(this._stdFile[i], i + 1, true) != EErrorCode.NONE)
                    {
                        return false;
                    }
                    if (DataCenter._fileCompare.LoadMsrtFromFile(this._msrtFile[i], i + 1, true) != EErrorCode.NONE)
                    {
                        return false;
                    }
                }
                else
                {
                    if (DataCenter._fileCompare.LoadStdFromFile(this._stdFile[i], i + 1) != EErrorCode.NONE)
                    {
                        return false;
                    }
                    if (DataCenter._fileCompare.LoadMsrtFromFile(this._msrtFile[i], i + 1) != EErrorCode.NONE)
                    {
                        return false;
                    }
                }
            }
            this._stdFile.Clear();
            this._msrtFile.Clear();
            this.UpdateToUI();
            return true;
        }

        private void UpdateToUI()
        {
            this.dgvOpenFile.Rows.Clear();

            for (int i = 0; i < this._stdFile.Count; i++)
            {
                this.dgvOpenFile.Rows.Add();
                this.dgvOpenFile.Rows[i].Cells[0].Value = (i + 1).ToString();
                this.dgvOpenFile.Rows[i].Cells[1].Value = Path.GetFileName(this._stdFile[i]);
            }

            for (int i = 0; i < this._msrtFile.Count; i++)
            {
                if (i >= this.dgvOpenFile.Rows.Count)
                {
                    this.dgvOpenFile.Rows.Add();
                }
                this.dgvOpenFile.Rows[i].Cells[2].Value = Path.GetFileName(this._msrtFile[i]);
            }
        }

    }
}
